/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PayableTypeController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payable types and their catalogues.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Payables.UseCases;

namespace Empiria.Payments.Payables.WebApi {

  /// <summary>Web API used to retrive and update payable types and their catalogues.</summary>
  public class PayableTypeController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/payments-management/payables/payable-types")]
    [Route("v2/payments-management/payable-types")]
    public CollectionModel GetPayableTypes() {

      using (var usecases = PayableUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> payableTypes = usecases.GetPayableTypes();

        return new CollectionModel(Request, payableTypes);
      }
    }

    #endregion Query web apis

  }  // class PayableTypeController

}  // namespace Empiria.Payments.Payables.WebApi
