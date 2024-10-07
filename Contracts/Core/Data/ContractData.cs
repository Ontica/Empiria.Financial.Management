   /* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Data Layer                              *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data service                            *
*  Type     : ContractData                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract instances.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Contracts.Data {

  /// <summary>Provides data read and write methods for contract instances.</summary>
  static internal class ContractData {

    #region Methods

    static internal FixedList<Contract> getContracts(string filter, string sortBy) {
      var sql = "select * from pym_contracts ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" where {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" order by {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Contract>(dataOperation);

    }

    static internal void WriteContract(Contract o, string extensionData) {
      var op = DataOperation.Parse("write_PYM_Contract",
                     o.Id, o.UID, o.ContractType.Id, o.ContractNo, o.Name,
                     o.Description, o.Currency.Id, o.FromDate, o.ToDate, o.SignDate,
                     o.ManagedByOrgUnit.Id, o.BudgetType.Id, o.Supplier.Id, o.Parent.Id,
                     extensionData, o.Keywords,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class ContractData

}  // namespace Empiria.Contracts.Data
