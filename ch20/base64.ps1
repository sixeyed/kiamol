function Convert-ToBase64 {
	param (
		[parameter(ValueFromPipeline)]
		[string] $text,
		[switch] $d
	)
	if ($d){
		[Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($text))
	}
	else {
		[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($text))
	}
}

Set-Alias base64  Convert-ToBase64