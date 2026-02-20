/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Information holder                      *
*  Type     : BudgetAccount                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Financial account that represents a budget account.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Parties;
using Empiria.StateEnums;

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
      BudgetProgram = BudgetProgram.ParseWithCode(orgUnit.ExtendedData.Get<string>("budgetProgram"));
    }

    static public new BudgetAccount Parse(int id) => ParseId<BudgetAccount>(id);

    static public new BudgetAccount Parse(string uid) => ParseKey<BudgetAccount>(uid);


    static public BudgetAccount TryParse(string accountNo) =>
          TryParse<BudgetAccount>($"ACCT_NUMBER = '{accountNo}'");


    static public BudgetAccount TryParse(OrganizationalUnit orgUnit, string accountNo) =>
          TryParse<BudgetAccount>($"ACCT_NUMBER = '{accountNo}' AND ACCT_ORG_UNIT_ID = {orgUnit.Id}");


    static public new BudgetAccount Empty => ParseEmpty<BudgetAccount>();


    static public FixedList<BudgetAccount> GetList(BudgetType budgetType, OrganizationalUnit orgUnit) {
      Assertion.Require(budgetType, nameof(budgetType));
      Assertion.Require(orgUnit, nameof(orgUnit));

      return GetList().FindAll(x => x is BudgetAccount account &&
                                    account.BudgetType.Equals(budgetType) &&
                                    account.OrganizationalUnit.Equals(orgUnit))
                   .Select(x => (BudgetAccount) x)
                   .ToFixedList();
    }

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
        return $"{StandardAccount.Name}" +
               (Status == EntityStatus.Pending ? " (Autorización pendiente)" : string.Empty);
      }
    }


    public BudgetProgram BudgetProgram {
      get {
        return BudgetProgram.ParseWithCode(ExtData.Get<string>("budgetProgram", "N/D"));
      }
      private set {
        ExtData.SetIfValue("budgetProgram", value.Code);
      }
    }


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(base.Keywords, BudgetProgram.Name);
      }
    }


    #endregion Properties

    #region Methods

    internal new void SetStatus(EntityStatus newStatus) {

      if (Status == EntityStatus.Pending && newStatus == EntityStatus.OnReview) {
        var programCode = OrganizationalUnit.ExtendedData.Get("budgetProgram", string.Empty);

        if (programCode.Length != 0) {
          BudgetProgram = BudgetProgram.ParseWithCode(programCode);
        } else {
          BudgetProgram = BudgetProgram.Undefined;
        }

      } else if (Status == EntityStatus.OnReview && newStatus == EntityStatus.Pending) {
        BudgetProgram = BudgetProgram.Undefined;
      }

      base.SetStatus(newStatus);
    }

    #endregion Methods

  } // class BudgetAccount

} // namespace Empiria.Budgeting
