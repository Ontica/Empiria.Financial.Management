/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialConceptGroupController              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve financial concepts through their groups.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Concepts.Adapters;
using Empiria.Financial.Concepts.UseCases;

namespace Empiria.Financial.Concepts.WebApi {

  /// <summary>Web API used to retrieve financial concepts through their groups.</summary>
  public class FinancialConceptGroupController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v3/financial-concepts/groups")]
    public CollectionModel GetFinancialConceptsGroups() {

      using (var usecases = FinancialConceptGroupUseCases.UseCaseInteractor()) {

        FixedList<FinancialConceptGroupDescriptor> groups = usecases.GetGroups();

        return new CollectionModel(base.Request, groups);
      }
    }


    [HttpGet]
    [Route("v3/financial-concepts/list/{groupNamedKey}")]
    public CollectionModel GetFinancialConceptsList([FromUri] string groupNamedKey) {

      using (var usecases = FinancialConceptGroupUseCases.UseCaseInteractor()) {

        FixedList<FinancialConceptEntityDto> concepts = usecases.GetConceptsList(groupNamedKey);

        return new CollectionModel(base.Request, concepts);
      }
    }


    [HttpPost]
    [Route("v3/financial-concepts/groups/{groupUID:guid}")]
    public SingleObjectModel SearchConcepts([FromUri] string groupUID,
                                            [FromBody] FinancialConceptsQuery query) {

      query.GroupUID = groupUID;

      using (var usecases = FinancialConceptGroupUseCases.UseCaseInteractor()) {

        FinancialConceptGroupDto groupConcepts = usecases.SearchConcepts(query);

        return new SingleObjectModel(base.Request, groupConcepts);
      }
    }

    #endregion Query web apis

  }  // class FinancialConceptGroupController

}  // namespace Empiria.Financial.Concepts.WebApi
