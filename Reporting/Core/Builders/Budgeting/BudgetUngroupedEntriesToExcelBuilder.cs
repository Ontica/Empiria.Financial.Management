/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetUngroupedEntriesToExcelBuilder          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with ungrouped budget transaction entries.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds an Excel file with ungrouped budget transaction entries.</summary>
  internal class BudgetUngroupedEntriesToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public BudgetUngroupedEntriesToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<BudgetTransaction> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(transactions);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<BudgetTransaction> transactions) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var txn in transactions) {
        foreach (var entry in txn.Entries) {
          _excelFile.SetCell($"A{i}", txn.BaseBudget.Name);
          _excelFile.SetCell($"B{i}", txn.TransactionNo);
          _excelFile.SetCell($"C{i}", txn.BudgetTransactionType.DisplayName);
          _excelFile.SetCell($"D{i}", txn.Status.GetName());
          _excelFile.SetCell($"E{i}", entry.BudgetAccount.OrganizationalUnit.Code);
          _excelFile.SetCell($"F{i}", entry.BudgetAccount.OrganizationalUnit.Name);
          _excelFile.SetCell($"G{i}", entry.BudgetAccount.Code);
          _excelFile.SetCell($"H{i}", entry.BudgetAccount.Name);
          _excelFile.SetCell($"I{i}", entry.Description);
          _excelFile.SetCell($"J{i}", entry.BudgetProgram.Code);
          _excelFile.SetCell($"K{i}", entry.ControlNo);
          _excelFile.SetCell($"L{i}", entry.Year);
          _excelFile.SetCell($"M{i}", entry.Month);
          _excelFile.SetCell($"N{i}", entry.BalanceColumn.Name);
          _excelFile.SetCell($"O{i}", entry.Deposit);
          _excelFile.SetCell($"P{i}", entry.Withdrawal);
          _excelFile.SetCell($"Q{i}", entry.Justification);
          i++;
        }  //  foreach entry
      } // foreach txn
    }

  } // class BudgetUngroupedEntriesToExcelBuilder

} // namespace Empiria.Budgeting.Reporting
