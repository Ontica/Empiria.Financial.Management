/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : CashTransactionToExcelBuilder                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with cash ledger transactions and their entries.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

using Empiria.CashFlow.CashLedger.Adapters;

namespace Empiria.Financial.Reporting {

  /// <summary>Builds an Excel file with cash ledger transactions and their entries.</summary>
  internal class CashTransactionToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public CashTransactionToExcelBuilder(FileTemplateConfig templateConfig) {
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

        foreach (CashTransactionEntryDto entry in holder.Entries) {

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
          _excelFile.SetCell($"K{i}", entry.AccountNumber);
          _excelFile.SetCell($"L{i}", entry.AccountName);
          _excelFile.SetCell($"M{i}", entry.SubledgerAccountNumber);
          _excelFile.SetCell($"N{i}", entry.SubledgerAccountName);
          _excelFile.SetCell($"O{i}", entry.SectorCode);
          _excelFile.SetCell($"P{i}", entry.VerificationNumber);
          _excelFile.SetCell($"Q{i}", entry.ResponsibilityAreaCode);
          _excelFile.SetCell($"R{i}", entry.CurrencyName);

          if (entry.CurrencyId != DEFAULT_CURRENCY_ID) {
            _excelFile.SetCell($"S{i}", entry.ExchangeRate);
          }

          _excelFile.SetCellIfValue($"T{i}", entry.Debit);
          _excelFile.SetCellIfValue($"U{i}", entry.Credit);

          _excelFile.SetCell($"V{i}", entry.CashAccount.Name);
          _excelFile.SetCell($"W{i}", entry.CuentaSistemaLegado);


          _excelFile.SetCell($"X{i}", 1);
          _excelFile.SetCellIfValue($"Y{i}", entry.Pending ? 1 : 0);
          _excelFile.SetCellIfValue($"Z{i}", entry.Exact ? 1 : 0);
          _excelFile.SetCellIfValue($"AA{i}", entry.Correct ? 1 : 0);
          _excelFile.SetCellIfValue($"AB{i}", entry.FalsePositive ? 1 : 0);
          _excelFile.SetCell($"AC{i}", entry.CashAccountAppliedRule);

          i++;

        }  //  foreach entry

      } // foreach holder
    }

  } // class CashTransactionToExcelBuilder

} // namespace Empiria.Financial.Reporting
