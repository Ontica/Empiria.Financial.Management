/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments validator services                Component : payment notfication                     *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Payment validator processor             *
*  Type     : PaymentValidator                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a workflow status change of a land transaction.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Threading;
using Empiria.Payments.Orders.Adapters;
using Empiria.Payments.Orders;
using Empiria.Payments.Processor.Services;


namespace Empiria.Payments.Processor {

  /// <summary>Represents a payment instruction.</summary>
  internal class PaymentValidator {

    private static bool isRunning = false;
    private static volatile Timer timer = null;

    #region Properties

    static public bool IsRunning {
      get {
        return isRunning;
      }
    }


    #endregion Properties

    #region Methods

    /// <summary>Starts the execution engine for pending messages in asyncronous mode.</summary>
    static public void Start() {
      try {
        if (isRunning) {
          return;
        }

        int MESSAGE_ENGINE_EXECUTION_MINUTES = ConfigurationData.Get("MessageEngine.Execution.Minutes", 2);
        timer = new Timer(ValidatePayment, null, 10 * 1000,
                          MESSAGE_ENGINE_EXECUTION_MINUTES * 60 * 1000);

        isRunning = true;
        EmpiriaLog.Info("PaymentValidator was started.");

      } catch (Exception e) {
        EmpiriaLog.Info("PaymentValidator was stopped due to an ocurred exception.");

        EmpiriaLog.Error(e);

        isRunning = false;
      }
    }


    /// <summary>Stops the execution engine.</summary>
    static public void Stop() {
      if (!isRunning) {
        return;
      }
      timer.Dispose();
      timer = null;
      isRunning = false;

      EmpiriaLog.Info("PaymentValidator was stopped.");
    }


    #endregion Methods

    #region Helpers

    static private void ValidatePayment(object stateInfo) {
      var paymentInstructions = PaymentInstruction.GetInProccessPaymentInstructions();
      int count = 0;

      foreach (var paymentInstruction in paymentInstructions) {
        count++;
        using (var usecases = PaymentService.ServiceInteractor()) {
          usecases.UpdatePaymentInstructionStatus(paymentInstruction);
        }
      }

    }

    #endregion Helpers

  }  // class PaymentValidator

} // namespace Empiria.Payments.Processor
