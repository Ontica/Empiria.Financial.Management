/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Flow Management                          Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : CashTransactionTotalsToExcelBuilder           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with cash ledger transactions totals.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Financial;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.CashFlow.Reporting {

  /// <summary>Builds an Excel file with cash ledger transactions totals.</summary>
  internal class CashTransactionTotalsToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public CashTransactionTotalsToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<CashTransactionHolderDto> transactions) {
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


    private void FillOut(FixedList<CashTransactionHolderDto> holders) {
      int DEFAULT_CURRENCY_ID = Currency.Default.Id;

      int i = _templateConfig.FirstRowIndex;

      foreach (var holder in holders) {
        var txn = holder.Transaction;

        foreach (var groupByCurrency in holder.Entries.GroupBy(x => x.CurrencyId)
                                                      .OrderBy(x => x.Key)) {

          _excelFile.SetCell($"A{i}", txn.LedgerName);
          _excelFile.SetCell($"B{i}", txn.Number);
          _excelFile.SetCell($"C{i}", txn.AccountingDate);
          _excelFile.SetCell($"D{i}", txn.RecordingDate);
          _excelFile.SetCell($"E{i}", txn.TransactionTypeName);
          _excelFile.SetCell($"F{i}", txn.VoucherTypeName);
          _excelFile.SetCell($"G{i}", txn.SourceName);
          _excelFile.SetCell($"H{i}", txn.ElaboratedBy);
          _excelFile.SetCell($"I{i}", txn.Id);
          _excelFile.SetCell($"J{i}", txn.Concept);

          _excelFile.SetCell($"K{i}", groupByCurrency.ElementAt(0).CurrencyName);

          var entries = groupByCurrency.ToFixedList().FindAll(x => x.CashFlowAssigned);

          _excelFile.SetCellIfValue($"L{i}", entries.Count);
          _excelFile.SetCellIfValue($"M{i}", entries.Sum(x => x.Debit));
          _excelFile.SetCellIfValue($"N{i}", entries.Sum(x => x.Credit));
          _excelFile.SetCellIfValue($"O{i}", entries.Sum(x => x.Debit - x.Credit));

          entries = groupByCurrency.ToFixedList().FindAll(x => x.NoCashFlow);

          _excelFile.SetCellIfValue($"P{i}", entries.Count);
          _excelFile.SetCellIfValue($"Q{i}", entries.Sum(x => x.Debit));
          _excelFile.SetCellIfValue($"R{i}", entries.Sum(x => x.Credit));
          _excelFile.SetCellIfValue($"S{i}", entries.Sum(x => x.Debit - x.Credit));

          entries = groupByCurrency.ToFixedList().FindAll(x => x.Pending || x.CashFlowUnassigned);

          _excelFile.SetCellIfValue($"T{i}", entries.Count);
          _excelFile.SetCellIfValue($"U{i}", entries.Sum(x => x.Debit));
          _excelFile.SetCellIfValue($"V{i}", entries.Sum(x => x.Credit));
          _excelFile.SetCellIfValue($"W{i}", entries.Sum(x => x.Debit - x.Credit));

          var legadoConFlujo = groupByCurrency.ToFixedList().FindAll(x => x.LegadoConFlujo);

          _excelFile.SetCellIfValue($"X{i}", legadoConFlujo.Count);
          _excelFile.SetCellIfValue($"Y{i}", legadoConFlujo.Sum(x => x.Debit));
          _excelFile.SetCellIfValue($"Z{i}", legadoConFlujo.Sum(x => x.Credit));

          var legadoSinFlujo = groupByCurrency.ToFixedList().FindAll(x => x.LegadoSinFlujo);

          _excelFile.SetCellIfValue($"AA{i}", legadoSinFlujo.Count);
          _excelFile.SetCellIfValue($"AB{i}", legadoSinFlujo.Sum(x => x.Debit));
          _excelFile.SetCellIfValue($"AC{i}", legadoSinFlujo.Sum(x => x.Credit));

          entries = groupByCurrency.ToFixedList().FindAll(x => x.CashFlowAssigned);

          _excelFile.SetCellIfValue($"AD{i}", entries.Count - legadoConFlujo.Count);
          _excelFile.SetCellIfValue($"AE{i}", entries.Sum(x => x.Debit) - legadoConFlujo.Sum(x => x.Debit));
          _excelFile.SetCellIfValue($"AF{i}", entries.Sum(x => x.Credit) - legadoConFlujo.Sum(x => x.Credit));

          entries = groupByCurrency.ToFixedList().FindAll(x => x.NoCashFlow);

          _excelFile.SetCellIfValue($"AG{i}", entries.Count - legadoSinFlujo.Count());
          _excelFile.SetCellIfValue($"AH{i}", entries.Sum(x => x.Debit) - legadoSinFlujo.Sum(x => x.Debit));
          _excelFile.SetCellIfValue($"AI{i}", entries.Sum(x => x.Credit) - legadoSinFlujo.Sum(x => x.Credit));

          entries = groupByCurrency.ToFixedList();

          _excelFile.SetCellIfValue($"AJ{i}", entries.Count);
          _excelFile.SetCellIfValue($"AK{i}", entries.Count(x => x.Pending));
          _excelFile.SetCellIfValue($"AL{i}", entries.Count(x => x.Exact));
          _excelFile.SetCellIfValue($"AM{i}", entries.Count(x => x.Correct));
          _excelFile.SetCellIfValue($"AN{i}", entries.Count(x => x.FalsePositive));

          i++;

        }  //  foreach entry

      } // foreach holder
    }

  } // class CashTransactionTotalsToExcelBuilder

} // namespace Empiria.CashFlow.Reporting
