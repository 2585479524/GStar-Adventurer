---------------------------------------------------------------------------------
TypeA Simple Toon Shader
© 2016 Type74 nonact
Version 1.0
web    http://type74.rer.jp
mail   nonact@type74.rer.jp
---------------------------------------------------------------------------------

Toon01 (no outline)

Main Color          Overall color change.

Shadow              The color of the portion receiving the shadow.

Ambient             The color of the dark part.

Diffuse             The color of the bright part.

Highlight           The color of the highlights.

Ambient border      Value to determine the boundary between light and dark.

Ambient blur        Blurring degree of the boundary.

Highlight Power     The strength of the highlights.

Highlight Hardness  Highlights of blurriness.

Rim Intensity       The degree of influence of rimlight.

Rim Power           The strength of the rimlight.

Texture             RGB -> color.
                    A   -> Adjust the reflection.

---------------------------------------------------------------------------------

Toon01Edge (outline)

Additional3 points.

Edge Color            The color of the outline edge.

Edge Thickness        The thickness of the outline edge.
                      As the thickness is constant, calculated using the distance.

VertexColor to Edge   To adjust the thickness using the vertexcolor.
                      R thin. く 0.0 < 0.5 < 1.0 thick.
                      G unused
                      B standard Z 0.0 < 1.0 Z value in the back.

---------------------------------------------------------------------------------

Hair (outline)


change point.

NU, NV      Stretch can pull the highlights in each direction.

Rotation    To rotate the direction out of the highlight.
            W is unused.


