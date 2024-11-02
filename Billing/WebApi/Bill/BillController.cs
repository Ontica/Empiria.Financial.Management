﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                      Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : BillController                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payable bills.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.WebApi;
using System.Web.Http;

using Empiria.Billing.UseCases;
using Empiria.Billing.Adapters;

namespace Empiria.Billing.WebApi {
  
  /// <summary></summary>
  public class BillController : WebApiController {

    #region Web apis


    [HttpPost]
    [Route("v2/billing-management/bills/search")]
    public CollectionModel SearchBills([FromBody] BillsQuery query) {

      using (var usecases = BillUseCases.UseCaseInteractor()) {
        FixedList<BillDescriptorDto> payables = usecases.GetBillList(query);

        return new CollectionModel(this.Request, payables);
      }
    }


    #endregion Web apis

  } // class BillController

} // namespace Empiria.Billing.WebApi