/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Cost Object                        Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialCostObjectController                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update financial cost object.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.CostObject.Adapters;
using Empiria.Financial.CostObject.UseCases;

namespace Empiria.Financial.CostObject.WebApi {

  /// <summary>Web API used to retrieve and update financial concepts.</summary>
  public class FinancialCostObjectController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v3/costs-management/cost-centers")]
    [Route("v3/costs-management/cost-objects")]
    public CollectionModel GetCostObjects() {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FixedList<FinancialCostObjectDto> costObjects = usecases.GetCostObjects();

        return new CollectionModel(base.Request, costObjects);
      }
    }

    [HttpGet]
    [Route("v3/costs-management/cost-centers/{costObjectUID:guid}")]
    [Route("v3/costs-management/cost-objects/{costObjectUID:guid}")]
    public SingleObjectModel GetByUIDCostObject([FromUri] string costObjectUID) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FinancialCostObjectDto costObject = usecases.GetByUID(costObjectUID);

        return new SingleObjectModel(base.Request, costObject);
      }
    }

    [HttpPost]
    [Route("v3/costs-management/cost-centers/search")]
    [Route("v3/costs-management/cost-objects/search")]
    public CollectionModel SearchCostObjectsByOrgUnit([FromBody] FinancialCostObjectEntry query) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FixedList<FinancialCostObjectDto> costObject = usecases.SearchCostObjectsByOrgUnit(query);

        return new CollectionModel(base.Request, costObject);
      }
    }


    [HttpGet]
    [Route("v3/costs-management/cost-centers-types")]
    public CollectionModel GetCostObjectsTypes() {

      var typesCostObject = FinancialCostObjectType.GetList();

      return new CollectionModel(base.Request, typesCostObject);
    }


    [HttpPost]
    [Route("v3/costs-management/cost-centers")]
    [Route("v3/costs-management/cost-objects")]
    public SingleObjectModel CreateCostObject([FromBody] FinancialCostObjectEntry entry) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FinancialCostObjectDto costObject = usecases.CreateCostObject(entry);

        return new SingleObjectModel(base.Request, costObject);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v3/costs-management/cost-centers/{costObjectUID:guid}")]
    [Route("v3/costs-management/cost-objects/{costObjectUID:guid}")]
    public SingleObjectModel UpdateCostObject([FromUri] string costObjectUID,
                                              [FromBody] FinancialCostObjectEntry entry) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        var costObject = usecases.UpdateCostObject(costObjectUID, entry);

        return new SingleObjectModel(base.Request, costObject);
      }
    }

    [HttpDelete]
    [Route("v3/costs-management/cost-centers/{costObjectUID:guid}")]
    [Route("v3/costs-management/cost-objects/{costObjectUID:guid}")]
    public NoDataModel DeleteCostObject([FromUri] string costObjectUID) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        usecases.DeleteCostObject(costObjectUID);

        return new NoDataModel(base.Request);
      }
    }

    #endregion Query web apis

  }  // class FinancialCostObjectController

}  // namespace Empiria.Financial.CostObject.WebApi
