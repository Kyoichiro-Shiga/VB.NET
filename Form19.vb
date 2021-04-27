' 問題19
Public Class Form19

    Private mainGraphics As Graphics
    Private PlayerImage As Bitmap

    Private Const _PATH As String = "C:\temp\JWS\VB.NET\Practice19\"
    Private Const _SHIP As String = "ship.bmp"

    Dim x As Integer        ' 横軸（右方向に正、左方向に負）
    Dim y As Integer        ' 縦軸（下方向に正、上方向に負）
    Dim IsUp As Boolean     ' 上矢印が押されているか
    Dim IsDown As Boolean   ' 下矢印が押されているか
    Dim IsLeft As Boolean   ' 左矢印が押されているか
    Dim IsRight As Boolean  ' 右矢印が押されているか

    ' タイマー処理
    Private Sub Timer19_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick

        '描画されているものを消して全面を白くする。
        mainGraphics.Clear(Color.White)

        ' 上移動
        If IsUp AndAlso y >= PlayerImage.Height Then
            y -= PlayerImage.Height
        End If

        ' 下移動
        If IsDown AndAlso y <= ClientSize.Height - PlayerImage.Height Then
            y += PlayerImage.Height
        End If

        ' 左移動
        If IsLeft AndAlso x >= PlayerImage.Width Then
            x -= PlayerImage.Width
        End If

        ' 右移動
        If IsRight AndAlso x <= ClientSize.Width - PlayerImage.Width Then
            x += PlayerImage.Width
        End If

        ' 船を描画する。
        mainGraphics.DrawImage(PlayerImage, x, y)

        'フォームを再描画する。
        Invalidate()
    End Sub

    ' キーダウン処理
    Private Sub Form19_KeyDown(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown

        ' 入力内容
        Select Case e.KeyCode

            ' 「↑」キー
            Case Keys.Up
                IsUp = True

            ' 「↓」キー
            Case Keys.Down
                IsDown = True

            ' 「←」キー
            Case Keys.Left
                IsLeft = True

            ' 「→」キー
            Case Keys.Right
                IsRight = True
        End Select
    End Sub

    ' キーアップ処理
    Private Sub Form19_KeyUp(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp

        ' 入力内容
        Select Case e.KeyCode

            ' 「↑」キー
            Case Keys.Up
                IsUp = False

            ' 「↓」キー
            Case Keys.Down
                IsDown = False

            ' 「←」キー
            Case Keys.Left
                IsLeft = False

            ' 「→」キー
            Case Keys.Right
                IsRight = False
        End Select
    End Sub

    ' 初期表示処理
    Private Sub Form19_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' 初回のみ描画用のGraphicsクラスを生成する。
        If mainGraphics Is Nothing Then
            Dim bmp As New Bitmap(ClientRectangle.Width, ClientRectangle.Height)
            BackgroundImage = bmp
            mainGraphics = Graphics.FromImage(bmp)
        End If

        ' 画像ファイルを読み込む。
        PlayerImage = Image.FromFile(_PATH + _SHIP)
    End Sub
End Class
