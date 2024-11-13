/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                      Component : Web Api                               *
*  Assembly : Empiria.Assets.WebApi.dll                    Pattern   : Query Api Controller                  *
*  Type     : FixedAssetsController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve fixed assets data.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Assets.Adapters;
using Empiria.Assets.UseCases;

namespace Empiria.Assets.WebApi {

  /// <summary>Web API used to retrieve fixed assets data.</summary>
  public class FixedAssetsController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/fixed-assets/{fixedAssetUID:guid}")]
    public SingleObjectModel GetFixedAsset([FromUri] string fixedAssetUID) {

      using (var usecases = FixedAssetUseCases.UseCaseInteractor()) {
        FixedAssetHolderDto fixedAsset = usecases.GetFixedAsset(fixedAssetUID);

        return new SingleObjectModel(base.Request, fixedAsset);
      }
    }


    [HttpGet]
    [Route("v2/fixed-assets/types")]
    public CollectionModel GetFixedAssetTypes() {

      using (var usecases = FixedAssetUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> fixedAssetTypes = usecases.GetFixedAssetTypes();

        return new CollectionModel(base.Request, fixedAssetTypes);
      }
    }


    [HttpPost]
    [Route("v2/fixed-assets/search")]
    public CollectionModel SearchFixedAssets([FromBody] FixedAssetsQuery query) {

      base.RequireBody(query);

      using (var usecases = FixedAssetUseCases.UseCaseInteractor()) {
        FixedList<FixedAssetDescriptor> fixedAssets = usecases.SearchFixedAssets(query);

        return new CollectionModel(base.Request, fixedAssets);
      }
    }

    #endregion Web Apis

  }  // class FixedAssetsController

}  // namespace Empiria.Assets.WebApi
