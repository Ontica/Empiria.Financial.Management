/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                      Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : BillController                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payable bills.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;
using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;
using Empiria.Documents;
using Empiria.WebApi;

namespace Empiria.Billing.WebApi {

  /// <summary>Web API used to retrive and update payable bills.</summary>
  public class BillController : WebApiController {

    #region Web apis

    [HttpGet]
    [Route("v2/billing-management/bills/bills-structure")]
    public SingleObjectModel GetBill([FromBody] string[] billsUID) {

      using (var service = BillUseCases.UseCaseInteractor()) {

        BillsStructureDto bill = service.GetBills(billsUID);

        return new SingleObjectModel(base.Request, bill);
      }
    }


    [HttpGet]
    [Route("v2/billing-management/bills/bill-types-for-edition")]
    public CollectionModel GetBillTypes() {

      var billTypes = DocumentProduct.GetList<DocumentProduct>()
                     .FindAll(x => x.InternalCode.StartsWith("BILL-") &&
                              x.ApplicationContentType.Equals("complemento-pago-sat"))
                     .ToFixedList()
                     .Select(x => BillTypeDto.MapToBillTypeDto(x))
                     .ToFixedList();

      return new CollectionModel(base.Request, billTypes);
    }

    #endregion Web apis

  } // class BillController

} // namespace Empiria.Billing.WebApi
