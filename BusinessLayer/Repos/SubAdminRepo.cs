using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.DTO;
using AjoorLiteWebAPI.Core;

namespace BusinessLayer.Repos
{
    public class SubAdminRepo
    {
        static AjoEntities entities = new AjoEntities();
        public bool AddSubAdmin(SubAdmin subAdmin)
        {
            var model = new cor_sub_admin()
            {
                Firstname = subAdmin.Firstname,
                Lastname = subAdmin.Lastname,
                FullName = $"{subAdmin.Firstname} {subAdmin.Lastname}",
                Password = subAdmin.Password,
                Username = subAdmin.Username,
                Email = subAdmin.Email,
                PhoneNo = subAdmin.PhoneNo,
                CreatedBy = subAdmin.CreatedBy,
                CreatedDate = subAdmin.CreatedDate,
                UpdatedBy = subAdmin.UpdatedBy,
                UpdatedDate = subAdmin.UpdatedDate
            };
            entities.cor_sub_admin.Add(model);
            return entities.SaveChanges() > 0;
        }

        public bool DeleteSubAdmin(List<long> IDs)
        {
            foreach (var ID in IDs)
            {
                var model = entities.cor_sub_admin.Find(ID);
                if (model != null)
                {
                    entities.cor_sub_admin.Remove(model);
                }
            }
            return entities.SaveChanges() > 0;
        }

        public void LogInUser(long ID, string Fullname)
        {
            var loginDetails = new cor_sub_admin_login_log()
            {
                SubId = ID,
                FullName = Fullname,
                LoginDate = DateTime.Now
            };
            entities.cor_sub_admin_login_log.Add(loginDetails);
            entities.SaveChanges();
        }

        public bool UpdateSubAdmin(SubAdmin subAdmin)
        {
            var updateModel = entities.cor_sub_admin.Find(subAdmin.SubId);
            if (updateModel != null)
            {
                updateModel.Firstname = subAdmin.Firstname;
                updateModel.Lastname = subAdmin.Lastname;
                updateModel.FullName = $"{subAdmin.Firstname} {subAdmin.Lastname}";
                updateModel.Username = subAdmin.Username;
                updateModel.PhoneNo = subAdmin.PhoneNo;
                updateModel.UpdatedBy = subAdmin.UpdatedBy;
                updateModel.Email = subAdmin.Email;
                updateModel.UpdatedDate = subAdmin.UpdatedDate;
            }
            return entities.SaveChanges() > 0;
        }

        public SubAdmin GetSubAdmin(long SubAdminId)
        {
            var model = entities.cor_sub_admin.Find(SubAdminId);
            if (model != null)
            {
                SubAdmin SubAdmin = new SubAdmin()
                {
                    SubId = model.SubId,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    FullName = model.FullName,
                    Username = model.Username,
                    Email = model.Email,
                    PhoneNo = model.PhoneNo,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = model.CreatedDate,
                    UpdatedBy = model.UpdatedBy,
                    UpdatedDate = model.UpdatedDate
                };
                return SubAdmin;
            }
            return null;
        }

        public IQueryable<ListOfSubAdmin> GetAllRecords()
        {
            var query = from item in entities.cor_sub_admin
                        select new ListOfSubAdmin()
                        {
                            Firstname = item.Firstname,
                            Lastname = item.Lastname,
                            FullName = item.FullName,
                            PhoneNo = item.PhoneNo,
                            SubId = item.SubId,
                            Username = item.Username,
                            Email = item.Email,
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate,
                            UpdatedBy = item.UpdatedBy,
                            UpdatedDate = item.UpdatedDate
                        };
            return query.AsQueryable();
        }

        public bool ValidateUserCredentials(string username, string password)
        {
            var user = entities.cor_sub_admin.Where(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (user != null)
            {
                var decryptedPassword = Cryptography.Decrypt(user.Password, "SuperiorInvestment#");
                if (decryptedPassword.Equals(password))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DoesNumberExists(string number)
        {
            if (entities.cor_sub_admin.Any(x => x.PhoneNo == number))
            {
                return true;
            }
            if (entities.cor_customer.Any(x => x.PhoneNumber == number))
            {
                return true;
            }
            return false;
        }

        public bool DoesEmailExists(string email)
        {
            if (entities.cor_sub_admin.Any(x => x.Email == email))
            {
                return true;
            }
            if (entities.cor_customer.Any(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        public bool DoesUsernameExist(string username)
        {
            if (!entities.cor_sub_admin.Any(x => x.Username == username))
            {
                return false;
            }
            return true;
        }

        public bool ChangePassword(string username, string newPassword)
        {
            var user = entities.cor_sub_admin.Where(x => x.Username == username).FirstOrDefault();
            if (user != null)
            {
                user.Password = Cryptography.Encrypt(newPassword, "SuperiorInvestment#");
            }
            return entities.SaveChanges() > 0;
        }
    }
}
