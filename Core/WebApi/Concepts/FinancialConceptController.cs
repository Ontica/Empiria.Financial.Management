/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Web Api                               *
*  Assembly : Empiria.Financial.WebApi.dll                 Pattern   : Web api Controller                    *
*  Type     : FinancialConceptController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update financial concepts.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Concepts.Adapters;
using Empiria.Financial.Concepts.UseCases;

namespace Empiria.Financial.Concepts.WebApi {

  /// <summary>Web API used to retrieve and update financial concepts.</summary>
  public class FinancialConceptController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v3/financial-concepts/{conceptUID:guid}")]
    public SingleObjectModel GetFinancialConcept([FromUri] string conceptUID) {

      using (var usecases = FinancialConceptUseCases.UseCaseInteractor()) {

        FinancialConceptHolder concept = usecases.GetConcept(conceptUID);

        return new SingleObjectModel(base.Request, concept);
      }
    }

    #endregion Query web apis

  }  // class FinancialConceptController

}  // namespace Empiria.Financial.Concepts.WebApi
