using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token {

    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string userName { get; set; }
    public string scope { get; set; }
}
