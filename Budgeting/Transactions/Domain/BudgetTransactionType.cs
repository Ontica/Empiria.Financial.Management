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

namespace Empiria.Budgeting.Transactions {

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

    static public FixedList<BudgetTransactionType> GetList(BudgetType budgetType) {
      Assertion.Require(budgetType, nameof(budgetType));

      return GetList().FindAll(x => x.BudgetType.Equals(budgetType));
    }

    static public BudgetTransactionType ApartarGastoCorriente => Parse("ObjectTypeInfo.BudgetTransaction.ApartarGastoCorriente");

    static public BudgetTransactionType ApartarCostoFinanciero => Parse("ObjectTypeInfo.BudgetTransaction.ApartarCostoFinanciero");

    static public BudgetTransactionType ComprometerGastoCorriente => Parse("ObjectTypeInfo.BudgetTransaction.ComprometerGastoCorriente");

    static public BudgetTransactionType ComprometerCostoFinanciero => Parse("ObjectTypeInfo.BudgetTransaction.ComprometerCostoFinanciero");

    static public BudgetTransactionType EjercerGastoCorriente => Parse("ObjectTypeInfo.BudgetTransaction.EjercerGastoCorriente");

    static public BudgetTransactionType EjercerCostoFinanciero => Parse("ObjectTypeInfo.BudgetTransaction.EjercerCostoFinanciero");

    static public BudgetTransactionType Empty => Parse("ObjectTypeInfo.BudgetTransaction");

    #endregion Constructors and parsers

    #region Properties

    public BudgetType BudgetType {
      get {
        int budgetTypeId = ExtensionData.Get<int>("budgetTypeId");

        return BudgetType.Parse(budgetTypeId);
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

    #endregion Properties

  } // class BudgetTransactionType

}  // namespace Empiria.Budgeting.Transactions
