/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Power Type                              *
*  Type     : BudgetType                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Ontology;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting {

  /// <summary>Power type that describes a budget.</summary>
  [Powertype(typeof(Budget))]
  public class BudgetType : Powertype {

    #region Constructors and parsers

    private BudgetType() {
      // Empiria power types always have this constructor.
    }

    static public new BudgetType Parse(int typeId) => Parse<BudgetType>(typeId);

    static public new BudgetType Parse(string typeName) => Parse<BudgetType>(typeName);

    static public FixedList<BudgetType> GetList() {
      return BaseBudgetType.ExtensionData.GetFixedList<BudgetType>("budgetTypes");
    }

    static private ObjectTypeInfo BaseBudgetType => Powertype.Parse("ObjectTypeInfo.PowerType.BudgetType");

    static public BudgetType Empty => Parse("ObjectTypeInfo.Budget");

    #endregion Constructors and parsers

    #region Properties

    public Currency Currency {
      get {
        return ExtensionData.Get("currencyId", Currency.Default);
      }
    }

    public bool Multiyear {
      get {
        return ExtensionData.Get("multiyear", false);
      }
    }

    public int PaymentsWithdrawalAccountId {
      get {
        return ExtensionData.Get<int>("paymentsWithdrawalAccountId");
      }
    }

    public string Prefix {
      get {
        return ExtensionData.Get("prefix", string.Empty);
      }
    }


    public FixedList<StandardAccountCategory> StdAccountCategories {
      get {
        return base.ExtensionData.GetFixedList<StandardAccountCategory>("stdAccountCategories");
      }
    }


    public StandardAccountType StandardAccountType {
      get {
        return base.ExtensionData.Get("standardAccountTypeId", StandardAccountType.Empty);
      }
    }


    internal FixedList<BudgetTransactionType> TransactionTypes {
      get {
        FixedList<int> ids = base.ExtensionData.GetFixedList<int>("transactionTypes");

        return ids.Select(x => BudgetTransactionType.Parse(x))
                  .ToFixedList();
      }
    }

    #endregion Properties

  }  // class BudgetType

}  // namespace Empiria.Budgeting
