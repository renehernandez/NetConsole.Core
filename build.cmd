IF NOT EXIST .paket\paket.exe (
    .paket\paket.bootstrapper.exe
    if errorlevel 1 (
      exit "There was an error with paket.bootstrapper"
    )
)

IF EXIST paket.lock (
    .paket\paket.exe restore
    if errorlevel 1 (
      exit "There was an error restoring packages from paket.lock"
    )
)

IF EXIST init.fsx (
  .paket\paket.exe install
  packages\FAKE\tools\FAKE.exe init.fsx
)
packages\FAKE\tools\FAKE.exe build.fsx %*
