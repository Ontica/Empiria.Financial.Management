/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : BudgetAccountUseCases                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for budget accounts.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.Parties;
using Empiria.Services;

namespace Empiria.Budgeting.UseCases {

  /// <summary>Use cases for budget account segments.</summary>
  public class BudgetAccountUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetAccountUseCases() {
      // no-op
    }

    static public BudgetAccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BudgetAccountUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public StandardAccountHolder CreateAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var stdAccount = StandardAccount.Parse(fields.StandardAccountUID);

      FinancialAccountType accountType;
      if (stdAccount.StandardAccountType.Id == 3213) {
        accountType = FinancialAccountType.Parse(3243);
      } else if (stdAccount.StandardAccountType.Id == 3214) {
        accountType = FinancialAccountType.Parse(3244);
      } else {
        throw Assertion.EnsureNoReachThisCode("The standard account provided is not valid for budget accounts.");
      }

      var orgUnit = OrganizationalUnit.Parse(fields.OrganizationalUnitUID);

      var existing = FinancialAccount.GetList(x => x.StandardAccount.Equals(stdAccount) &&
                                                   x.OrganizationalUnit.Equals(orgUnit));

      Assertion.Require(existing.Count == 0,
        $"El área {orgUnit.FullName} ya tiene asignada la partida presupuestal {stdAccount.Name}.");


      var account = new BudgetAccount(accountType, stdAccount, orgUnit);

      account.Save();

      return StandardAccountMapper.Map(account.StandardAccount);
    }

    #endregion Use cases

  }  // class BudgetAccountUseCases

}  // namespace Empiria.Budgeting.UseCases
