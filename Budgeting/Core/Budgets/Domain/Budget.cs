﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Partitioned Type + Aggregate Root       *
*  Type     : Budget                                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a budget and serves as an aggregate root for its accounts.    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Budgeting {

  /// <summary>Partitioned type that represents a budget and serves as an aggregate root
  /// for its accounts.</summary>
  [PartitionedType(typeof(BudgetType))]
  public class Budget : GeneralObject {

    #region Constructors and parsers

    protected Budget(BudgetType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected Budget() {
      // Required by Empiria Framework.
    }


    static public Budget Parse(int id) => ParseId<Budget>(id);

    static public Budget Parse(string uid) => ParseKey<Budget>(uid);

    static public FixedList<Budget> GetList() {
      return BaseObject.GetList<Budget>(string.Empty, "ObjectName")
                       .FindAll(x => x.Status != StateEnums.EntityStatus.Deleted)
                       .ToFixedList()
                       .Sort((x, y) => x.Year.CompareTo(y.Year))
                       .Reverse();
    }


    static public Budget Empty => ParseEmpty<Budget>();

    #endregion Constructors and parsers

    #region Properties

    public BudgetType BudgetType {
      get {
        return (BudgetType) base.GetEmpiriaType();
      }
    }


    public int Year {
      get {
        return base.ExtendedDataField.Get<int>("year");
      }
    }

    #endregion Properties

  }  // class Budget

}  // namespace Empiria.Budgeting