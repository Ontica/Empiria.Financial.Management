/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Power type                              *
*  Type     : BudgetAccountSegmentType                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget account segment partitioned type.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

using Empiria.Budgeting.Data;

namespace Empiria.Budgeting {

  /// <summary>Power type that describes a budget account segment partitioned type.</summary>
  [Powertype(typeof(BudgetAccountSegment))]
  public sealed class BudgetAccountSegmentType : Powertype {

    #region Constructors and parsers

    private BudgetAccountSegmentType() {
      // Empiria power types always have this constructor.
    }

    static public new BudgetAccountSegmentType Parse(int typeId) => Parse<BudgetAccountSegmentType>(typeId);

    static public new BudgetAccountSegmentType Parse(string typeName) => Parse<BudgetAccountSegmentType>(typeName);

    static public BudgetAccountSegmentType Empty => Parse("ObjectTypeInfo.BudgetAccountSegment");

    #endregion Constructors and parsers

    #region Properties

    public BudgetType BudgetType {
      get {
        return base.ExtensionData.Get("budgetTypeId", BudgetType.Empty);
      }
    }

    public bool IsEmptyInstance {
      get {
        return this.Equals(Empty);
      }
    }

    public string AsChildrenName {
      get {
        return base.ExtensionData.Get("asChildrenName", base.DisplayPluralName);
      }
    }


    public string AsParentName {
      get {
        return base.ExtensionData.Get("asParentName", base.DisplayName);
      }
    }


    public BudgetAccountSegmentType ChildrenSegmentType {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        return base.ExtensionData.Get("childrenSegmentTypeId", Empty);
      }
    }


    public bool HasParentSegmentType {
      get {
        return !ParentSegmentType.IsEmptyInstance;
      }
    }


    public bool HasChildrenSegmentType {
      get {
        return !ChildrenSegmentType.IsEmptyInstance;
      }
    }


    public BudgetAccountSegmentType ParentSegmentType {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        return base.ExtensionData.Get("parentSegmentTypeId", Empty);
      }
    }


    public FixedList<BudgetAccountSegment> SearchInstances(string filterString, string keywords) {
      filterString = filterString ?? string.Empty;
      keywords = keywords ?? string.Empty;

      return BudgetAccountSegmentDataService.BudgetAccountSegments(this, filterString, keywords);
    }

    #endregion Properties

  } // class BudgetAccountSegmentType

} // namespace Empiria.Budgeting
