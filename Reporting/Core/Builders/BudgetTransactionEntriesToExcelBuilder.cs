/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetTransactionEntriesToExcelBuilder        License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with all budget transaction entries.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;

namespace Empiria.Financial.Reporting {

  /// <summary>Builds an Excel file with all budget transaction entries.</summary>
  internal class BudgetTransactionEntriesToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public BudgetTransactionEntriesToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<BudgetTransactionByYear> transactions) {
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


    private void FillOut(FixedList<BudgetTransactionByYear> transactions) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var txn in transactions) {
        foreach (var entry in txn.GetEntries()) {
          _excelFile.SetCell($"A{i}", txn.Transaction.BaseBudget.Name);
          _excelFile.SetCell($"B{i}", txn.Transaction.TransactionNo);
          _excelFile.SetCell($"C{i}", txn.Transaction.BudgetTransactionType.DisplayName);
          _excelFile.SetCell($"D{i}", txn.Transaction.Status.GetName());
          _excelFile.SetCell($"E{i}", entry.BudgetAccount.Code);
          _excelFile.SetCell($"F{i}", entry.BudgetAccount.BaseSegment.Name);
          _excelFile.SetCell($"G{i}", entry.BudgetAccount.BudgetProgram);
          _excelFile.SetCell($"H{i}", entry.Year);
          _excelFile.SetCell($"I{i}", entry.BalanceColumn.Name);
          _excelFile.SetCell($"J{i}", entry.Total);
          _excelFile.SetCell($"K{i}", entry.GetAmountForMonth(1));
          _excelFile.SetCell($"L{i}", entry.GetAmountForMonth(2));
          _excelFile.SetCell($"M{i}", entry.GetAmountForMonth(3));
          _excelFile.SetCell($"N{i}", entry.GetAmountForMonth(4));
          _excelFile.SetCell($"O{i}", entry.GetAmountForMonth(5));
          _excelFile.SetCell($"P{i}", entry.GetAmountForMonth(6));
          _excelFile.SetCell($"Q{i}", entry.GetAmountForMonth(7));
          _excelFile.SetCell($"R{i}", entry.GetAmountForMonth(8));
          _excelFile.SetCell($"S{i}", entry.GetAmountForMonth(9));
          _excelFile.SetCell($"T{i}", entry.GetAmountForMonth(10));
          _excelFile.SetCell($"U{i}", entry.GetAmountForMonth(11));
          _excelFile.SetCell($"V{i}", entry.GetAmountForMonth(12));
          _excelFile.SetCell($"W{i}", entry.Justification);
          i++;
        }  //  foreach entry
      } // foreach txn
    }

  } // class BudgetTransactionEntriesToExcelBuilder

} // namespace Empiria.FinancialAccounting.Reporting.Balances
