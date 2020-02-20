
for $i = 0 to 100
WinActivate("The Steganographer")
WinWaitActive("The Steganographer")
MouseClick("left", 840, 269,1,0)
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
Send($i & (Random(100000, 999999) * 100 * $i))
MouseClick("left", 517, 443,1,0)
WinWaitActive("The Steganographer")
MouseClick("left", 823, 468,1,0)
Next