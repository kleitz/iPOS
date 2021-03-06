﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iPOS.Web.Service.Interface;
using iPOS.Web.Database;
using iPOS.Web.Models;
using iPOS.Web.Repository;
using iPOS.Web.Service;

namespace iPOS.Web.Areas.Administrator.Controllers
{
    public class PawningController : Controller
    {
        #region PUBLIC CONSTRUCTOR
        private readonly IPawningService _pawningService;
        private readonly IAppraisalService _appraisalService;
        private readonly ICustomerService _customerService;
        private readonly IReferenceService _referenceService;

        public PawningController(
            IPawningService pawningService,
            IAppraisalService appraisalService,
            ICustomerService customerServic,
            IReferenceService referenceService)
        {
            _pawningService = pawningService;
            _appraisalService = appraisalService;
            _customerService = customerServic;
            _referenceService = referenceService;
        }
        public PawningController()
        {
            _pawningService = new PawningService(new UnitOfWorkFactory());
            _appraisalService = new AppraisalService(new UnitOfWorkFactory());
            _customerService = new CustomerService(new UnitOfWorkFactory());
            _referenceService = new ReferenceService(new UnitOfWorkFactory());
        }
        #endregion

        #region VIEW
        // GET: Administrator/Pawning
        public ActionResult Index()
        {
            ViewBag.Form = "Pawning";
            ViewBag.Controller = "Pawning";
            ViewBag.Action = "Module";

            return View();
        }
        #endregion

        #region JSON REQUEST METHODS
        // GET
        [HttpGet]
        [Route("Administrator/Customer/GetCustomerList")]
        public async Task<JsonResult> GetPawnedItems(int page, int pageSize)
        {
            var listPawnedItem = await _pawningService.GetList(page, pageSize);
            var result =
                from a in listPawnedItem
                select new
                {
                    a.PawnedItemId,
                    a.PawnedItemNo,
                    a.PawnedDate,
                    a.AppraiseId,
                    a.CustomerId,
                    a.PawnedItemContractNo,
                    a.LoanableAmount,
                    a.InterestRate,
                    a.InterestAmount,
                    a.InitialPayment,
                    a.ServiceCharge,
                    a.Others,
                    a.IsInterestDeducted,
                    a.NetCashOut,
                    a.TermsId,
                    a.ScheduleOfPayment,
                    a.NoOfPayments,
                    a.DueDateStart,
                    a.DueDateEnd,
                    a.IsReleased,
                    a.ReviewedBy,
                    a.ApprovedBy,
                    a.CreatedBy,
                    a.CreatedAt
                };

            return Json(new { data = result.OrderByDescending(d => d.PawnedDate).ThenBy(s => s.IsReleased), noMoreData = result.Count() < pageSize, recordCount = result.Count() }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAppraisedItem()
        {
            var listAppraisedItem = await _appraisalService.GetList();
            var result =
                from a in listAppraisedItem
                where a.IsPawned.Equals(false)
                select new
                {
                    a.AppraiseId,
                    a.ItemName
                };

            return Json(result.OrderBy(o => o.ItemName), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCustomer()
        {
            var listCustomer = await _customerService.GetCustomerList();
            var result = listCustomer.Select(item => new customer()
            {
                Id = item.Id,
                FirstName = item.FirstName + " " + item.LastName,
                LastName = item.LastName
            });

            return Json(result.OrderBy(o => o.LastName), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAppraisedItemById(int AppraisedItemId)
        {
            var listAppraisedItem = await _appraisalService.FindByIdList(AppraisedItemId);
            var listItemType = await _referenceService.GetItemTypeList();
            var listItemCategory = await _referenceService.GetItemCategoryList();

            var result = from a in listAppraisedItem
            join b in listItemType on a.ItemTypeId equals b.ItemTypeId
            join c in listItemCategory on a.ItemCategoryId equals c.ItemCategoryId
            where a.IsPawned.Equals(false)
            select new
            {
                a.AppraiseId,
                b.ItemTypeName,
                c.ItemCategoryName,
                a.Weight,
                a.AppraisedValue,
                a.Remarks
            };

            return Json(new { data = result.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCustomerById(int CustomerId)
        {
            var listCustomer = await _customerService.FindByIdCustomer(CustomerId);

            return Json(listCustomer, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}