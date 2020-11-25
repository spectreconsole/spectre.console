Title: Canvas
Order: 4
---

`Canvas` is a widget that allows you to render arbitrary "pixels" 
(or _coxels_, as [Simon Cropp](https://twitter.com/SimonCropp/status/1331554791726534657?s=20) 
suggested we should call them).

# Drawing primitives

```csharp
// Create a canvas
var canvas = new Canvas(16, 16);

// Draw some shapes
for(var i = 0; i < canvas.Width; i++)
{
    // Cross
    canvas.SetPixel(i, i, Color.White);
    canvas.SetPixel(canvas.Width - i - 1, i, Color.White);

    // Border
    canvas.SetPixel(i, 0, Color.Red);
    canvas.SetPixel(0, i, Color.Green);
    canvas.SetPixel(i, canvas.Height - 1, Color.Blue);
    canvas.SetPixel(canvas.Width - 1, i, Color.Yellow);
}

// Render the canvas
AnsiConsole.Render(canvas);
```

## Result

<pre style="font-size:100%;font-family:consolas,'Courier New',monospace;line-height: normal; padding: 0px;background-color: #222222; padding: 20px;">
<span style="background-color: #008000">  </span><span style="background-color: #FF0000">                              </span>
<span style="background-color: #008000">  </span><span style="background-color: #800080">  </span><span>                        </span><span style="background-color: #800080">  </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>  </span><span style="background-color: #800080">  </span><span>                    </span><span style="background-color: #800080">  </span><span>  </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>    </span><span style="background-color: #800080">  </span><span>                </span><span style="background-color: #800080">  </span><span>    </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>      </span><span style="background-color: #800080">  </span><span>            </span><span style="background-color: #800080">  </span><span>      </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>        </span><span style="background-color: #800080">  </span><span>        </span><span style="background-color: #800080">  </span><span>        </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>          </span><span style="background-color: #800080">  </span><span>    </span><span style="background-color: #800080">  </span><span>          </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>            </span><span style="background-color: #800080">    </span><span>            </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>            </span><span style="background-color: #800080">    </span><span>            </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>          </span><span style="background-color: #800080">  </span><span>    </span><span style="background-color: #800080">  </span><span>          </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>        </span><span style="background-color: #800080">  </span><span>        </span><span style="background-color: #800080">  </span><span>        </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>      </span><span style="background-color: #800080">  </span><span>            </span><span style="background-color: #800080">  </span><span>      </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>    </span><span style="background-color: #800080">  </span><span>                </span><span style="background-color: #800080">  </span><span>    </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span>  </span><span style="background-color: #800080">  </span><span>                    </span><span style="background-color: #800080">  </span><span>  </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span style="background-color: #800080">  </span><span>                        </span><span style="background-color: #800080">  </span><span style="background-color: #FFFF00">  </span>
<span style="background-color: #008000">  </span><span style="background-color: #0000FF">                            </span><span style="background-color: #FFFF00">  </span>
</pre>