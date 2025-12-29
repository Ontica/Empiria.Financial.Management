/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Power type                              *
*  Type     : BudgetTransactionType                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget transaction.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Budgeting.Transactions {

  public enum MultiplicityRule {

    None,

    OnePerYear,

    ZeroOrOnePerYear,

  }

  /// <summary>Power type that describes a budget transaction.</summary>
  [Powertype(typeof(BudgetTransaction))]
  public sealed class BudgetTransactionType : Powertype {

    #region Constructors and parsers

    private BudgetTransactionType() {
      // Empiria powertype types always have this constructor.
    }

    static public new BudgetTransactionType Parse(int typeId) => Parse<BudgetTransactionType>(typeId);

    static public new BudgetTransactionType Parse(string typeName) => Parse<BudgetTransactionType>(typeName);

    static public FixedList<BudgetTransactionType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (BudgetTransactionType) x)
            .ToFixedList();
    }


    static public BudgetTransactionType GetFor(BudgetType budgetType,
                                               BudgetOperationType operationType) {
      Assertion.Require(budgetType, nameof(budgetType));

      var transactionType = GetList()
                           .First(x => x.BudgetType.Equals(budgetType) &&
                                       x.OperationType == operationType);

      Assertion.Require(transactionType,
                        $"Operation type '{operationType}' is not defined " +
                        $"for budget type '{budgetType.Name}'.");

      return transactionType;
    }


    static public FixedList<BudgetTransactionType> GetList(BudgetType budgetType) {
      Assertion.Require(budgetType, nameof(budgetType));

      return GetList().FindAll(x => x.BudgetType.Equals(budgetType));
    }

    static public BudgetTransactionType Empty => Parse("ObjectTypeInfo.BudgetTransaction");

    #endregion Constructors and parsers

    #region Properties

    public bool AllowsMultiYearEntries {
      get {
        return ExtensionData.Get("allowsMultiYearEntries", false);
      }
    }


    public FixedList<int> AvailableYears {
      get {
        return ExtensionData.GetFixedList<int>("availableYears", false);
      }
    }


    public FixedList<BalanceColumn> BalanceColumns {
      get {
        return ExtensionData.GetFixedList<BalanceColumn>("balanceColumns");
      }
    }


    public string BudgetAccountsFilter {
      get {
        return ExtensionData.Get("accountsFilter", string.Empty);
      }
    }


    public BudgetType BudgetType {
      get {
        int budgetTypeId = ExtensionData.Get<int>("budgetTypeId");

        return BudgetType.Parse(budgetTypeId);
      }
    }


    public bool IsProtected {
      get {
        return ExtensionData.Get("isProtected", false);
      }
    }


    public bool ManualEdition {
      get {
        return ExtensionData.Get("manualEdition", false);
      }
    }


    public MultiplicityRule MultiplicityRule {
      get {
        return ExtensionData.Get("multiplicityRule", MultiplicityRule.None);
      }
    }


    public FixedList<OperationSource> OperationSources {
      get {
        return ExtensionData.GetFixedList<OperationSource>("sources", false)
                            .Sort((x, y) => x.Name.CompareTo(y.Name));
      }
    }


    public BudgetOperationType OperationType {
      get {
        return ExtensionData.Get("operationType", BudgetOperationType.None);
      }
    }


    public string Prefix {
      get {
        return ExtensionData.Get("prefix", string.Empty);
      }
    }


    public FixedList<ObjectTypeInfo> RelatedDocumentTypes {
      get {
        var ids = ExtensionData.GetFixedList<int>("relatedDocumentTypes", false);

        return ids.Select(x => ObjectTypeInfo.Parse(x))
                  .ToFixedList();
      }
    }


    public bool SelectParty {
      get {
        return ExtensionData.Get("selectParty", false);
      }
    }


    public ThreeStateValue SelectProduct {
      get {
        return ExtensionData.Get("selectProduct", ThreeStateValue.False);
      }
    }

    #endregion Properties

  } // class BudgetTransactionType

}  // namespace Empiria.Budgeting.Transactions
