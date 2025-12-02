/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : FuelConsumptionBillMapper                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for fuel consumption bill fields.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Billing.SATMexicoImporter;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for fuel consumption bill fields</summary>
  static internal class FuelConsumptionBillMapper {

    #region Public methods

    static internal IBillFields Map(SATFuelConsumptionBillDto paymentComplementDto) {

      return MapToFuelConsumptionBillFields(paymentComplementDto);
    }

    #endregion Public methods


    #region Private methods

    static private IBillFields MapToFuelConsumptionBillFields(
                                SATFuelConsumptionBillDto paymentComplementDto) {

      throw new NotImplementedException();
    }

    #endregion Private methods

  } // class FuelConsumptionBillMapper

} // namespace Empiria.Billing.Adapters
