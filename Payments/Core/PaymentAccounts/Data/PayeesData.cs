/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PayeesData                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for Payee instances.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Payments.Data {

  /// <summary>Provides data read and write methods for Payee instances.</summary>
  static internal class PayeesData {

    static internal int GetNextPayeeId() {
      var sql = "SELECT MIN(Party_Id) MinPartyId FROM Parties";

      var op = DataOperation.Parse(sql);

      return (int) DataReader.GetScalar<decimal>(op) - 1;
    }


    static internal FixedList<Payee> SearchPayees(string filter, string sortBy) {
      var sql = "SELECT * FROM Parties";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Payee>(op);
    }

  }  // class PayeesData

}  // namespace Empiria.Payments.Data
