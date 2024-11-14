/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                      Component : Web Api                               *
*  Assembly : Empiria.Assets.WebApi.dll                    Pattern   : Query Api Controller                  *
*  Type     : FixedAssetsTransactionsController            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve fixed assets transactions data.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Assets.UseCases;

namespace Empiria.Assets.WebApi {

  /// <summary>Web API used to retrieve fixed assets transactions data.</summary>
  public class FixedAssetsTransactionsController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/fixed-assets/transactions/types")]
    public CollectionModel GetFixedAssetTransactionTypes() {

      using (var usecases = FixedAssetTransactionUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> transactionTypes = usecases.GetFixedAssetTransactionTypes();

        return new CollectionModel(base.Request, transactionTypes);
      }
    }

    #endregion Web Apis

  }  // class FixedAssetsTransactionsController

}  // namespace Empiria.Assets.WebApi
