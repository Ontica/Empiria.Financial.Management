/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases interactor                  *
*  Type     : FinancialConceptUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for retrieve and update financial concepts.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Concepts.Adapters;

namespace Empiria.Financial.Concepts.UseCases {

  /// <summary>Provides use cases for retrieve and update financial concepts.</summary>
  public class FinancialConceptUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialConceptUseCases() {
      // no-op
    }

    static public FinancialConceptUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialConceptUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FinancialConceptHolder GetConcept(string conceptUID) {
      Assertion.Require(conceptUID, nameof(conceptUID));

      var concept = FinancialConcept.Parse(conceptUID);

      return FinancialConceptMapper.MapToHolder(concept);
    }

    #endregion Use cases

  }  // class FinancialConceptUseCases

}  // namespace Empiria.Financial.Concepts.UseCases
