using DataLayer;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.DTO;

namespace BusinessLayer.Repos
{
    public class CustomerRepo
    {
        AjoEntities entities = new AjoEntities();

        public bool AddCustomer(Customer customer)
        {
            var model = new cor_customer()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                FullName = $"{customer.FirstName} {customer.LastName}",
                AccountNumber = customer.AccountNumber,
                Email = customer.Email,
                Product = customer.Product,
                Commission = customer.Commission,
                CreatedBy = customer.CreatedBy,
                CreatedDate = customer.CreatedDate,
                PhoneNumber = customer.PhoneNumber
            };
            entities.cor_customer.Add(model);
            return entities.SaveChanges() > 0;
        }

        public bool DeleteCustomer(List<long> IDs)
        {
            foreach (var ID in IDs)
            {
                var model = entities.cor_customer.Find(ID);
                if (model != null)
                {
                    var transactions = entities.cor_transactions.Where(x => x.CustomerId == ID);
                    entities.cor_transactions.RemoveRange(transactions);
                    entities.cor_customer.Remove(model);
                }
            }
            return entities.SaveChanges() > 0;
        }

        public bool UpdateCustomer(Customer customer)
        {
            var updateModel = entities.cor_customer.Find(customer.CustomerId);
            if (updateModel != null)
            {
                updateModel.FirstName = customer.FirstName;
                updateModel.LastName = customer.LastName;
                updateModel.FullName = $"{customer.FirstName} {customer.LastName}";
                updateModel.Email = customer.Email;
                updateModel.Product = customer.Product;
                updateModel.PhoneNumber = customer.PhoneNumber;
                updateModel.Commission = customer.Commission;
                updateModel.UpdatedBy = customer.UpdatedBy;
                updateModel.UpdateDate = customer.UpdateDate;
            }
            return entities.SaveChanges() > 0;
        }

        public bool ChangeCustomerCreator(Customer customer)
        {
            var recordToUpdate = entities.cor_customer.Find(customer.CustomerId);
            recordToUpdate.CreatedBy = customer.CreatedBy;
            return entities.SaveChanges() > 0;
        }

        public Customer GetCustomer(long customerId)
        {
            var model = entities.cor_customer.Find(customerId);
            if (model != null)
            {
                Customer customer = new Customer()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = model.FullName,
                    AccountNumber = model.AccountNumber,
                    PhoneNumber = model.PhoneNumber,
                    CustomerId = model.CustomerId,
                    Product = model.Product,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = model.CreatedDate,
                    Email = model.Email,
                    Commission = model.Commission,
                    UpdateDate = model.UpdateDate,
                    UpdatedBy = model.UpdatedBy
                };
                return customer;
            }
            return null;
        }

        public IQueryable<Customer> GetAllRecords()
        {
            var query = from item in entities.cor_customer
                        select new Customer()
                        {
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            FullName = item.FullName,
                            AccountNumber = item.AccountNumber,
                            CustomerId = item.CustomerId,
                            PhoneNumber = item.PhoneNumber,
                            Email = item.Email,
                            Product = item.Product,
                            Commission = item.Commission,
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate,
                            UpdatedBy = item.UpdatedBy,
                            UpdateDate = item.UpdateDate
                        };
            return query.AsQueryable();
        }

        public bool DoesNumberExists(string number)
        {
            if (entities.cor_customer.Any(x => x.PhoneNumber == number))
            {
                return true;
            }
            if (entities.cor_sub_admin.Any(x => x.PhoneNo == number))
            {
                return true;
            }
            return false;
        }

        public bool DoesEmailExists(string email)
        {
            if (entities.cor_customer.Any(x => x.Email == email))
            {
                return true;
            }
            if (entities.cor_sub_admin.Any(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        public int GetLastAssignedAccountNumber()
        {
            var model = entities.cor_customer.OrderByDescending(x => x.CustomerId).FirstOrDefault();
            if (model != null)
            {
                return model.AccountNumber;
            }
            return 0;
        }
    }
}
