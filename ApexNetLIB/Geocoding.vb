Imports System
Imports System.Net
Imports System.Xml.XPath



Public Class Geocoding

    Public Shared Function GetCoordinate(ByVal Indirizzo As String, ByRef Latitudine As Double, ByRef Longitudine As Double) As Boolean

        ' Documentation: https://developers.google.com/maps/documentation/geocoding/

        Dim requestUri As String = String.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(Indirizzo))

        Dim url As New Uri(requestUri)

        ' Dim url As New Uri("http://maps.googleapis.com/maps/api/geocode/" + "xml?address=" + Indirizzo.Replace(" ", "+") + "&sensor=false")


        Dim response As WebResponse
        Dim request As HttpWebRequest
        Dim document As XPathDocument
        Dim navigator As XPathNavigator

        Dim statusIterator As XPathNodeIterator
        Dim resultIterator As XPathNodeIterator
        Dim formattedAddressIterator As XPathNodeIterator
        Dim latIterator As XPathNodeIterator
        Dim geometryIterator As XPathNodeIterator
        Dim lngIterator As XPathNodeIterator
        Dim locationTypeIterator As XPathNodeIterator
        Dim locationIterator As XPathNodeIterator

        Try

            ' -----------------------------------------------------------
            request = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            response = request.GetResponse()

            If response IsNot Nothing Then

                document = New XPathDocument(response.GetResponseStream)
                navigator = document.CreateNavigator()

                ' get response status
                statusIterator = navigator.Select("/GeocodeResponse/status")

                While statusIterator.MoveNext()

                    If statusIterator.Current.Value = "ZERO_RESULTS" Then
                        Console.WriteLine("Warning: coordinate  not found = '" + statusIterator.Current.Value + "'")
                        Return False
                    End If

                    If statusIterator.Current.Value <> "OK" Then
                        Console.WriteLine("Error: response status = '" + statusIterator.Current.Value + "'")
                        Return False
                    End If
                End While

                ' get results
                resultIterator = navigator.Select("/GeocodeResponse/result")
                While resultIterator.MoveNext()

                    Console.WriteLine("Result: ")

                    formattedAddressIterator = resultIterator.Current.Select("formatted_address")
                    While formattedAddressIterator.MoveNext()

                        Console.WriteLine(" formatted_address: " + formattedAddressIterator.Current.Value)
                    End While

                    geometryIterator = resultIterator.Current.Select("geometry")
                    While geometryIterator.MoveNext()
                        '{
                        Console.WriteLine(" geometry: ")

                        locationIterator = geometryIterator.Current.Select("location")
                        While locationIterator.MoveNext()

                            Console.WriteLine("     location: ")

                            latIterator = locationIterator.Current.Select("lat")
                            While latIterator.MoveNext()
                                Console.WriteLine("         lat: " + latIterator.Current.Value)
                                Latitudine = latIterator.Current.ValueAsDouble
                            End While

                            lngIterator = locationIterator.Current.Select("lng")
                            While lngIterator.MoveNext()
                                Console.WriteLine("         lng: " + lngIterator.Current.Value)
                                Longitudine = lngIterator.Current.ValueAsDouble

                            End While
                        End While

                        locationTypeIterator = geometryIterator.Current.Select("location_type")
                        While locationTypeIterator.MoveNext()
                            Console.WriteLine("         location_type: " + locationTypeIterator.Current.Value)
                        End While
                    End While

                End While
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New Exception("SetProxy: " + ex.Message))
            Return False

        Finally

            If response IsNot Nothing Then
                response.Close()
                response = Nothing
            End If

        End Try

    End Function


End Class
