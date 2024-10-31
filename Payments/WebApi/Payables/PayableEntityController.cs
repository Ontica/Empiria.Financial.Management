/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PayableEntityController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive payable entities depending of their types.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Payables.Services;

namespace Empiria.Payments.Payables.WebApi {

  /// <summary>Web API used to retrive payable entities depending of their types.</summary>
  public class PayableEntityController : WebApiController {

    #region Query web apis

    [HttpPost]
    [Route("v2/payments-management/payable-entities/search")]
    public CollectionModel SearchPayableEntities([FromBody] PayableEntitiesQuery query) {

      using (var services = PayableEntityServices.ServiceInteractor()) {
        FixedList<PayableEntityDto> payableEntities = services.SearchPayableEntities(query);

        return new CollectionModel(Request, payableEntities);
      }
    }

    #endregion Query web apis

  }  // class PayableEntityController

}  // namespace Empiria.Payments.Payables.WebApi
