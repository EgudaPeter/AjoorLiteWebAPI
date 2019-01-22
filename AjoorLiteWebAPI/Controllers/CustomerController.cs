using BusinessLayer.DTO;
using BusinessLayer.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AjoorLiteWebAPI.Core;
using System.Threading;

namespace AjoorLiteWebAPI.Controllers
{
    [BasicAuthentication]
    public class CustomerController : ApiController
    {
        static CustomerRepo _CustomerRepo = new CustomerRepo();

        [HttpGet]
        [Route("api/getallcustomers")]
        public HttpResponseMessage GetAllCustomers()
        {
            try
            {
                var customers = _CustomerRepo.GetAllRecords();
                return Request.CreateResponse(HttpStatusCode.OK, customers);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/getcustomerbyquery/{query}")]
        public HttpResponseMessage GetCustomerByQuery(string query)
        {
            try
            {
                if (Utilities.EnsureNumericOnly(query))
                {
                    int accountNumber = int.Parse(query);
                    var customers = _CustomerRepo.GetAllRecords().Where(x => x.AccountNumber == accountNumber);
                    return Request.CreateResponse(HttpStatusCode.OK, customers);
                }
                else if (query == "All")
                {
                    var customers = _CustomerRepo.GetAllRecords();
                    return Request.CreateResponse(HttpStatusCode.OK, customers);
                }
                else
                {
                    var customers = _CustomerRepo.GetAllRecords().Where(x => x.FirstName == query || x.LastName == query || x.FullName.Contains(query));
                    return Request.CreateResponse(HttpStatusCode.OK, customers);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/getcustomer/{id}")]
        public HttpResponseMessage GetCustomer(int id)
        {
            try
            {
                var customer = _CustomerRepo.GetCustomer(id);
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/createcustomer")]
        public HttpResponseMessage CreateCustomer([FromBody]Customer customer)
        {
            try
            {
                customer.FullName = $"{customer.FirstName} {customer.LastName}";
                customer.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                customer.CreatedDate = DateTime.Now;
                if (_CustomerRepo.AddCustomer(customer))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Customer added successfully.");
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: Customer record is not saved.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/updatecustomer")]
        public HttpResponseMessage UpdateCustomer([FromBody]Customer customer)
        {
            try
            {
                if (_CustomerRepo.UpdateCustomer(customer))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Customer updated successfully");
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured.\n\n Error details:\n Customer record is not updated.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/deletecustomer")]
        public HttpResponseMessage DeleteCustomer([FromBody]CUSTOMERIDSOBJECT customerIdsObj)
        {
            try
            {
                _CustomerRepo.DeleteCustomer(customerIdsObj.IDs);
                return Request.CreateResponse(HttpStatusCode.OK, "Customer(s) deleted successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }
    }

    public class CUSTOMERIDSOBJECT
    {
        public List<long> IDs { get; set; }
    }
}
