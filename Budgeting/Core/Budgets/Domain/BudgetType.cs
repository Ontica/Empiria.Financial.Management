/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Power Type                              *
*  Type     : BudgetType                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

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

    static public BudgetType Empty => Parse("ObjectTypeInfo.Budget.Empty");

    #endregion Constructors and parsers

    #region Properties

    public FixedList<BudgetAccountSegmentType> SegmentTypes {
      get {
        return base.ExtensionData.GetFixedList<BudgetAccountSegmentType>("segmentTypes");
      }
    }

    #endregion Properties

  }  // class BudgetType

}  // namespace Empiria.Budgeting
