/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                      Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : BillTypeController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update bill types.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.WebApi;
using System.Web.Http;

namespace Empiria.Billing.WebApi {

  /// <summary></summary>
  public class BillTypeController : WebApiController {

    #region Web apis

    [HttpGet]
    [Route("v2/billing-management/bills/bill-categories")]
    public CollectionModel GetBillCategories() {

      var billTypes = BillCategory.GetList();

      return new CollectionModel(base.Request, billTypes.MapToNamedEntityList());
    }

    [HttpGet]
    [Route("v2/billing-management/bills/bill-types")]
    public CollectionModel GetBillTypes() {

      var billTypes = BillType.GetList();

      return new CollectionModel(base.Request, billTypes.MapToNamedEntityList());
    }

    #endregion Web apis

  } // class BillTypeController

} // namespace Empiria.Billing.WebApi
