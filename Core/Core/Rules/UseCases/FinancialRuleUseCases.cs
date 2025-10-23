/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                              Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialRuleUseCases                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial rules.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

namespace Empiria.Financial.Rules.UseCases {

  /// <summary>Provides use cases for update and retrieve financial rules.</summary>
  public class FinancialRuleUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialRuleUseCases() {
      // no-op
    }

    static public FinancialRuleUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialRuleUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> GetCategories() {
      return FinancialRuleCategory.GetList()
                                  .MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class FinancialRuleUseCases

}  // namespace Empiria.Financial.Rules.UseCases
