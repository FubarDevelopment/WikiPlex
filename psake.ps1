# Helper script for those who want to run
# psake without importing the module.
import-module .\3rdParty\psake\psake.psm1
invoke-psake -framework '4.0' @args
remove-module psake