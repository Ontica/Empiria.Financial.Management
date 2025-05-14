/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : ProjectUseCases                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial projects.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Data;
using Empiria.Financial.Projects.Adapters;
using Empiria.Services;

namespace Empiria.Financial.Accounts.UseCases {

  /// <summary>Provides use cases for update and retrieve financial projects.</summary>
  public class AccountUseCases : UseCase {

    #region Constructors and parsers

    protected AccountUseCases() {
      // no-op
    }

    static public AccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<AccountUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> SearchStandardAccounts(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<StandardAccount> stdAcccounts = StandardtAccountDataService.SearchStandardtAccounts(keywords);

      return stdAcccounts.MapToNamedEntityList();
    }


    public StandardAccount GetStandardAccount(string uid) {
      Assertion.Require(uid, nameof(uid));

     return StandardAccount.Parse(uid);
    }


    public FixedList<NamedEntityDto> GetStandardAccountCategories() {
      return StandardAccountCategory.GetList().MapToNamedEntityList();
    }


    public StandardAccountCategory GetStandardAccountCategory(string uid) {
      Assertion.Require(uid, nameof(uid));

      return StandardAccountCategory.Parse(uid);
    }

    #endregion Use cases

  }  // class ProjectUseCases

}  // namespace Empiria.Financial.Projects.UseCases
