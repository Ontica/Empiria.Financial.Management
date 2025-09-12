/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Information Holder                      *
*  Type     : DynamicDto, IQuery                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a dynamic result after a query execution.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;

namespace Empiria.CashFlow.Explorer.Adapters {

  /// <summary>Control interface used to determine query types.</summary>
  public interface IQuery {

    // no-op

  }  // interface IQuery



  /// <summary>Holds a dynamic result after a query execution.</summary>
  public class DynamicDto<T> {

    public IQuery Query {
      get; internal set;
    }

    public FixedList<DataTableColumn> Columns {
      get; internal set;
    }

    public FixedList<T> Entries {
      get; internal set;
    }

  }  // class DynamicDto<T>

}  // namespace Empiria.CashFlow.Explorer.Adapters
