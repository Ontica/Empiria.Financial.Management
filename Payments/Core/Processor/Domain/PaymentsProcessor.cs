/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments validator services                Component : payment notfication                     *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Payment validator processor             *
*  Type     : PaymentsProcessor                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to process payment instructions.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Threading;

namespace Empiria.Payments.Processor {

  /// <summary>Provides services to process payment instructions.</summary>
  internal class PaymentsProcessor {

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

        int MESSAGE_ENGINE_EXECUTION_MINUTES = ConfigurationData.Get("MessageEngine.Execution.Minutes", 1);
        timer = new Timer(RefreshPaymentInstructions, null,
                          TimeSpan.FromMinutes(MESSAGE_ENGINE_EXECUTION_MINUTES),
                          TimeSpan.FromMinutes(MESSAGE_ENGINE_EXECUTION_MINUTES));

        isRunning = true;

        EmpiriaLog.Info("PaymentsProcessor was started.");

      } catch (Exception e) {
        EmpiriaLog.Info("PaymentsProcessor was stopped due to an ocurred exception.");

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

      EmpiriaLog.Info("PaymentsProcessor was stopped.");
    }


    #endregion Methods

    #region Helpers

    static private async void RefreshPaymentInstructions(object stateInfo) {
      var instructions = PaymentInstruction.GetInProccessPaymentInstructions();

      using (var usecases = PaymentService.ServiceInteractor()) {

        foreach (var instruction in instructions) {
          await usecases.RefreshPaymentInstruction(instruction);
        }
      }
    }

    #endregion Helpers

  }  // class PaymentsExecutor

} // namespace Empiria.Payments.Processor
