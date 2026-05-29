/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Output DTO                              *
*  Type     : BudgetEntryByYearDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a budget entry for a whole year.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Budgeting.Explorer;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Output DTO used to return budget entries for a whole year.</summary>
  public class BudgetEntryByYearDto {

    public string UID {
      get; internal set;
    }

    public BudgetEntryDtoType EntryType {
      get;
    } = BudgetEntryDtoType.Annually;


    public string TransactionUID {
      get; internal set;
    }

    public NamedEntityDto Party {
      get; internal set;
    }

    public NamedEntityDto BalanceColumn {
      get; internal set;
    }


    public NamedEntityDto BudgetAccount {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto ProductUnit {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

    public NamedEntityDto Project {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public FixedList<BudgetMonthEntryDto> Amounts {
      get; internal set;
    }

  }  // BudgetEntryByYearDto



  /// <summary>Output DTO used to return a budget month entry.</summary>
  public class BudgetMonthEntryDto {

    public string BudgetEntryUID {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public decimal ProductQty {
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }


    static internal FixedList<BudgetMonthEntryDto> Map(FixedList<BudgetDataInColumns> budgetData,
                                                   Func<BudgetDataInColumns, decimal> amountValueFunc,
                                                   bool includeZeroMonths) {

      var list = budgetData.Select(x => new BudgetMonthEntryDto {
        Month = x.Month,
        Amount = amountValueFunc(x)
      }).ToFixedList();

      if (!includeZeroMonths) {
        return list;
      }

      var zerosMonths = EmpiriaMath.GetRange(1, 12)
                                   .ToFixedList()
                                   .FindAll(x => !list.Contains(y => y.Month == x))
                                   .Select(x => new BudgetMonthEntryDto {
                                     Month = x,
                                     Amount = 0
                                   }).ToFixedList();

      list = FixedList<BudgetMonthEntryDto>.MergeDistinct(list, zerosMonths);

      list.Sort((x, y) => x.Month.CompareTo(y.Month));

      return list;
    }

  }  // class BudgetMonthEntryFields

}  // namespace Empiria.Budgeting.Transactions.Adapters
