﻿using iPOS.Web.Database;
using iPOS.Web.Repository.Interface;
using iPOS.Web.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPOS.Web.Service
{
    public class CustomerService: ICustomerService
    {
        #region PRIVATE MEMBER VARIABLES
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        #endregion

        #region PUBLIC CONSTRUCTOR
        public CustomerService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
        #endregion

        #region PUBLIC MEMBER METHODS (CUSTOMER)
        public async Task<customer> FindByIdCustomer(int id)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    return await uow.CustomerRepository.FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<customer> FindByFirstnameLastnameCustomer(string firstname, string lastname)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var result = await uow.CustomerRepository.AllWithAsync(u => u.FirstName == firstname && u.LastName == lastname);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<customer>> GetCustomerList(
            int pageIndex = 0,
            int pageSize = 100)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var customerlist = await uow.CustomerRepository.AllWithAsync(null);

                    customerlist = customerlist
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize).ToList();

                    return customerlist.ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SaveCustomer(customer model)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var customer = await FindByFirstnameLastnameCustomer(model.FirstName, model.LastName);
                    if (customer == null)
                    {
                        uow.CustomerRepository.Insert(model);
                        await uow.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCustomer(customer model)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var customer = await FindByIdCustomer(model.Id);
                    if (customer != null)
                    {
                        uow.CustomerRepository.Update(model);
                        await uow.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCustomer(string id)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        uow.CustomerRepository.Delete(id);
                        await uow.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}