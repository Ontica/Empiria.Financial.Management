/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                         Component : Web Api                               *
*  Assembly : Empiria.Contracts.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : ContractsController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update contracts.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Contracts.Adapters;
using Empiria.Contracts.UseCases;

namespace Empiria.Contracts.WebApi {

  /// <summary>Web API used to retrive and update contracts</summary>
  public class ContractsController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/contracts/{contractUID:guid}")]
    public SingleObjectModel GetContract([FromUri] string contractUID) {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        ContractDto contract = usecases.GetContract(contractUID);

        return new SingleObjectModel(base.Request, contract);
      }
    }


    [HttpGet]
    [Route("v2/contracts/contract-types")]
    public CollectionModel GetContractTypes() {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> contractTypes = usecases.GetContractTypes();

        return new CollectionModel(base.Request, contractTypes);
      }
    }


    [HttpGet]
    [Route("v2/contracts/contract-unit-types")]
    public CollectionModel GetContracUnitTypes() {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> contractUnitTypes = usecases.GetContractUnit();

        return new CollectionModel(base.Request, contractUnitTypes);
      }
    }


    [HttpGet]
    [Route("v2/contracts/contract-cucop-types")]
    public CollectionModel GetContraCucopTypes() {

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> contractCucopTypes = usecases.GetContractCucop();

        return new CollectionModel(base.Request, contractCucopTypes);
      }
    }


    [HttpPost]
    [Route("v2/contracts/search")]
    public CollectionModel SearchContracts([FromBody] ContractQuery query) {

      base.RequireBody(query);

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        FixedList<ContractDescriptor> contracts = usecases.SearchContracts(query);

        return new CollectionModel(base.Request, contracts);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/contracts")]
    public SingleObjectModel AddContract([FromBody] ContractFields fields) {

      base.RequireBody(fields);

      using (var usecases = ContractUseCases.UseCaseInteractor()) {
        ContractDto contract = usecases.AddContract(fields);

        return new SingleObjectModel(base.Request, contract);
      }
    }

    #endregion Command web apis

  }  // class ContractsController

}  // namespace Empiria.Contracts.WebApi
