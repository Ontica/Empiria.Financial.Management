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

    public StandardAccount GetStandardAccount(string uid) {
      Assertion.Require(uid, nameof(uid));

      return StandardAccount.Parse(uid);
    }


    public FixedList<NamedEntityDto> GetStandardAccountByCategory(int categoryId) {
      Assertion.Require(categoryId, nameof(categoryId));

      return StandardAccountDataService.GetStdAcctByCategory(categoryId).MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetStandardAccountCategories() {
      return StandardAccountCategory.GetList()
                                    .MapToNamedEntityList();
    }


    public StandardAccountCategory GetStandardAccountCategory(string stdAccountCategoryUID) {
      Assertion.Require(stdAccountCategoryUID, nameof(stdAccountCategoryUID));

      return StandardAccountCategory.Parse(stdAccountCategoryUID);
    }


    public FixedList<NamedEntityDto> SearchStandardAccounts(string keywords) {
      keywords = keywords ?? string.Empty;

      return StandardAccountDataService.SearchStandardAccounts(keywords)
                                       .MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class StandardAccountUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
