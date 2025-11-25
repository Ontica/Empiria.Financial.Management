/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Information holder                      *
*  Type     : BudgetAccount                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Financial account that represents a budget account.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial;

namespace Empiria.Budgeting {

  /// <summary>Financial account that represents a budget account.</summary>
  public class BudgetAccount : FinancialAccount {

    #region Constructors and parsers

    protected BudgetAccount(FinancialAccountType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    public BudgetAccount(FinancialAccountType accountType,
                         StandardAccount standardAccount,
                         OrganizationalUnit orgUnit) : base(accountType, standardAccount, orgUnit) {
      // no-op
    }


    static public new BudgetAccount Parse(int id) => ParseId<BudgetAccount>(id);

    static public new BudgetAccount Parse(string uid) => ParseKey<BudgetAccount>(uid);

    static public BudgetAccount TryParse(string accountNo) => TryParse<BudgetAccount>($"ACCT_NUMBER = '{accountNo}'");

    static public new BudgetAccount Empty => ParseEmpty<BudgetAccount>();

    #endregion Constructors and parsers

    #region Properties

    public FinancialAccountType BudgetAccountType {
      get {
        return (FinancialAccountType) base.GetEmpiriaType();
      }
    }


    public BudgetType BudgetType {
      get {
        int budgetTypeId = StandardAccount.ChartOfAccounts.GetValue<int>("budgetTypeId");

        return BudgetType.Parse(budgetTypeId);
      }
    }


    public new string Name {
      get {
        return $"{StandardAccount.StdAcctNo} - {StandardAccount.Name}" +
               (Status == EntityStatus.Pending ? " (Autorización pendiente)" : string.Empty);
      }
    }


    public string BudgetProgram {
      get {
        return ExtData.Get("budgetProgram", "N/D");
      }
      private set {
        ExtData.SetIfValue("budgetProgram", value);
      }
    }


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(base.Keywords, BudgetProgram);
      }
    }


    #endregion Properties

    #region Methods

    internal new void SetStatus(EntityStatus newStatus) {

      if (Status == EntityStatus.Pending && newStatus == EntityStatus.OnReview) {
        BudgetProgram = OrganizationalUnit.ExtendedData.Get("budgetProgram", "N/A");

      } else if (Status == EntityStatus.OnReview && newStatus == EntityStatus.Pending) {
        BudgetProgram = "N/D";
      }

      base.SetStatus(newStatus);
    }

    #endregion Methods

  } // class BudgetAccount

} // namespace Empiria.Budgeting
