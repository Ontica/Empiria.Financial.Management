/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Power type                              *
*  Type     : BudgetSegmentType                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a BudgetSegmentItem partitioned type.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Budgeting {

  /// <summary>Power type that describes a BudgetSegmentItem partitioned type.</summary>
  [Powertype(typeof(BudgetSegmentItem))]
  public sealed class BudgetSegmentType : Powertype {

    #region Constructors and parsers

    private BudgetSegmentType() {
      // Empiria power types always have this constructor.
    }

    static public new BudgetSegmentType Parse(int typeId) {
      return ObjectTypeInfo.Parse<BudgetSegmentType>(typeId);
    }

    static public new BudgetSegmentType Parse(string typeName) {
      return BudgetAccountType.Parse<BudgetSegmentType>(typeName);
    }

    static public BudgetSegmentType Empty => BudgetSegmentType.Parse("ObjectTypeInfo.BudgetSegmentType");

    static public BudgetSegmentType ProjectType => BudgetSegmentType.Parse("ObjectTypeInfo.BudgetSegmentType.Proyecto");

    #endregion Constructors and parsers

    #region Properties

    public bool IsEmptyInstance {
      get {
        return this.Equals(BudgetSegmentType.Empty);
      }
    }

    public string AsChildrenName {
      get {
        return base.ExtensionData.Get<string>("asChildrenName", base.DisplayPluralName);
      }
    }


    public string AsParentName {
      get {
        return base.ExtensionData.Get<string>("asParentName", base.DisplayName);
      }
    }


    public BudgetSegmentType ChildrenSegmentType {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        return base.ExtensionData.Get<BudgetSegmentType>("childrenSegmentTypeId", BudgetSegmentType.Empty);
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


    public BudgetSegmentType ParentSegmentType {
      get {
        if (this.IsEmptyInstance) {
          return this;
        }

        return base.ExtensionData.Get<BudgetSegmentType>("parentSegmentTypeId", BudgetSegmentType.Empty);
      }
    }

    #endregion Properties

  } // class BudgetSegmentType

} // namespace Empiria.Budgeting
