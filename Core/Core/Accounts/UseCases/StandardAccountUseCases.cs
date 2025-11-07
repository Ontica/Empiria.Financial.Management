/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : StandardAccountUseCases                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve standar accounts.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Data;

namespace Empiria.Financial.Accounts.UseCases {

  /// <summary>Provides use cases for update and retrieve standar accounts.</summary>
  public class StandardAccountUseCases : UseCase {

    #region Constructors and parsers

    protected StandardAccountUseCases() {
      // no-op
    }

    static public StandardAccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<StandardAccountUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases


    public FixedList<NamedEntityDto> GetStandardAccountsInCategory(string stdAccountCategoryNamedKey) {
      Assertion.Require(stdAccountCategoryNamedKey, nameof(stdAccountCategoryNamedKey));

      var category = GetStandardAccountCategory(stdAccountCategoryNamedKey);

      return category.GetStandardAccounts()
                     .MapToNamedEntityList();
    }


    public StandardAccountCategory GetStandardAccountCategory(string stdAccountCategoryNamedKey) {
      Assertion.Require(stdAccountCategoryNamedKey, nameof(stdAccountCategoryNamedKey));

      return CommonStorage.ParseNamedKey<StandardAccountCategory>(stdAccountCategoryNamedKey);
    }


    public FixedList<NamedEntityDto> GetStandardAccountsSegments(string stdAccountCategoryNamedKey) {
      Assertion.Require(stdAccountCategoryNamedKey, nameof(stdAccountCategoryNamedKey));

      var category = GetStandardAccountCategory(stdAccountCategoryNamedKey);

      return StandardAccountSegment.GetList(category)
                                   .MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetStandardAccounts(string keywords, int maxLevel = 2) {
      Assertion.Require(maxLevel >= 1, nameof(maxLevel));

      keywords = keywords ?? string.Empty;

      return StandardAccountDataService.GetStandardAccounts(keywords)
                                       .FindAll(x => x.Level <= maxLevel)
                                       .MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class StandardAccountUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
