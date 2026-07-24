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
    [Route("v3/financial-costobject/costobjects/{costObjectUID:guid}")]
    public SingleObjectModel GetFinancialConcept([FromUri] string costObjectUID) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FinancialCostObjectDto costObject = usecases.GetByUID(costObjectUID);

        return new SingleObjectModel(base.Request, costObject);
      }
    }

    [HttpGet]
    [Route("v3/financial-costobject/costobjects")]
    public CollectionModel GetActiveCostObjects() {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {
        
        FixedList<FinancialCostObjectDto> costObjects = usecases.GetCostObjects();
        
        return new CollectionModel(base.Request, costObjects);
      }
    }

    [HttpPost]
    [Route("v3/financial-costobject/costobjects")]
    public SingleObjectModel CreateCostObject([FromBody] FinancialCostObjectEntry entry) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        FinancialCostObjectDto costObject = usecases.CreateCostObject(entry);

        return new SingleObjectModel(base.Request, costObject);
      }
    }

    [HttpPut, HttpPatch]
    [Route("v3/financial-costobject/costobjects/{uid}")]
    public SingleObjectModel UpdateCostObject([FromUri] string uid,
                                              [FromBody] FinancialCostObjectEntry entry) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        var costObject = usecases.UpdateCostObject(uid, entry);
        
        return new SingleObjectModel(base.Request, costObject);
      }
    }

    [HttpDelete]
    [Route("v3/financial-costobject/costobjects/{uid}")]
    public NoDataModel DeleteCostObject([FromUri] string uid) {

      using (var usecases = FinancialCostObjectUseCases.UseCaseInteractor()) {

        usecases.DeleteCostObject(uid);

        return new NoDataModel(base.Request);
      }
    }

    #endregion Query web apis

  }  // class FinancialCostObjectController

}  // namespace Empiria.Financial.CostObject.WebApi
