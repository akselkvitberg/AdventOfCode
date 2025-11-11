let getPath day =
    let sourceDir = __SOURCE_DIRECTORY__
    let sourceFile = __SOURCE_FILE__
    $"you called me from {sourceDir} - {sourceFile} - {day}"
