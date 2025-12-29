/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetEntryMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetEntry instances to data transfer objects.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.DynamicData;
using Empiria.StateEnums;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Describes a budget entry type.</summary>
  public enum BudgetEntryDtoType {

    Monthly,

    Annually,

  }  // enum BudgetEntryDtoType



  /// <summary>Maps BudgetEntry instances to data transfer objects.</summary>
  static internal class BudgetEntryMapper {

    #region Mappers

    static internal FixedList<BudgetEntryDto> Map(FixedList<BudgetEntry> entries) {
      return entries.Select(x => Map(x))
                    .ToFixedList();
    }


    static internal BudgetEntryDto Map(BudgetEntry entry) {
      return new BudgetEntryDto {
        UID = entry.UID,
        TransactionUID = entry.Transaction.UID,
        BudgetAccount = entry.BudgetAccount.MapToNamedEntity(),
        Product = entry.Product.MapToNamedEntity(),
        ProductUnit = entry.ProductUnit.MapToNamedEntity(),
        ProductQty = entry.ProductQty,
        Project = entry.Project.MapToNamedEntity(),
        Party = entry.BudgetAccount.OrganizationalUnit.MapToNamedEntity(),
        Description = entry.Description,
        Justification = entry.Justification,
        Year = entry.Year,
        Month = new NamedEntityDto(entry.Month.ToString(), entry.MonthName),
        Day = entry.Day,
        Currency = entry.Currency.MapToNamedEntity(),
        BalanceColumn = entry.BalanceColumn.MapToNamedEntity(),
        RequestedAmount = entry.RequestedAmount,
        Amount = entry.Amount,
        ExchangeRate = entry.ExchangeRate,
        Status = entry.Status.MapToNamedEntity()
      };
    }


    static internal FixedList<BudgetEntryDescriptorDto> MapToDescriptor(FixedList<BudgetEntry> entries) {
      var list = new List<BudgetEntryDescriptorDto>(entries.Count + 1);

      entries.Sort((x, y) => $"{x.BudgetAccount.Code}.{x.Year}.{x.Month.ToString("00")}"
                             .CompareTo($"{y.BudgetAccount.Code}.{y.Year}.{y.Month.ToString("00")}"));

      list.AddRange(entries.Select(x => MapToDescriptor(x)));

      list.AddRange(GetTotalEntries(entries));

      return list.ToFixedList();
    }

    #endregion Mappers

    #region Helpers

    static private FixedList<BudgetEntryDescriptorDto> GetTotalEntries(FixedList<BudgetEntry> entries) {
      var groups = entries.GroupBy(x => $"{x.Year}|{x.BalanceColumn.Name}");

      var list = new List<BudgetEntryDescriptorDto>(groups.Count());

      foreach (var group in groups) {
        var dto = MapToDescriptorTotals(group.First(), group.ToFixedList());
        list.Add(dto);
      }

      return list.ToFixedList();
    }


    static private BudgetEntryDescriptorDto MapToDescriptor(BudgetEntry entry) {
      return new BudgetEntryDescriptorDto {
        UID = entry.UID,
        BudgetAccountCode = entry.BudgetAccount.Code,
        BudgetAccountName = entry.BudgetAccount.Name,
        PartyName = entry.BudgetAccount.OrganizationalUnit.Code,
        Program = entry.BudgetProgram.Code,
        ProductCode = entry.ProductCode,
        Description = entry.ProductName,
        ControlNo = entry.ControlNo,
        Year = entry.Year,
        Month = entry.Month,
        MonthName = entry.MonthName,
        Day = entry.Day,
        ItemType = DataTableEntryType.Entry.ToString(),
        BalanceColumn = entry.BalanceColumn.Name,
        Deposit = entry.Deposit,
        Withdrawal = entry.Withdrawal,
      };
    }


    static private BudgetEntryDescriptorDto MapToDescriptorTotals(BudgetEntry pivot,
                                                                  FixedList<BudgetEntry> items) {
      return new BudgetEntryDescriptorDto {
        UID = $"{pivot.Year}|{pivot.BalanceColumn.Name}",
        BudgetAccountCode = string.Empty,
        BudgetAccountName = $"{pivot.BalanceColumn.Name} {pivot.Budget.Name}",
        PartyName = pivot.BudgetAccount.OrganizationalUnit.Code,
        Year = pivot.Year,
        ItemType = DataTableEntryType.Total.ToString(),
        BalanceColumn = pivot.BalanceColumn.Name,
        Deposit = items.Sum(x => x.Deposit),
        Withdrawal = items.Sum(x => x.Withdrawal)
      };
    }

    #endregion Helpers

  }  // class BudgetEntryMapper

}  // namespace Empiria.Budgeting.Transactions.Adapters
