'問題20
Public Class Form20

    Private mainGraphics As Graphics

    Private Const _PATH As String = "C:\temp\VB.NET\Practice20\"
    Private Const _BOMB As String = "bomb.bmp"
    Private Const _MISSILE As String = "missile.bmp"
    Private Const _SHIP As String = "ship.bmp"
    Private Const _UFO As String = "ufo.bmp"

    Private ShipImage As Bitmap
    Private UfoImage As Bitmap
    Private MissileImage As Bitmap
    Private BombImage As Bitmap

    Private Const _SCORE As String = "スコア："
    Private Const _ZERO_PADDING As String = "00000"
    Private Const _ADDITION As Integer = 100
    Private Const _SUBSTRACTION As Integer = 50

    Dim IScore As Integer = 0   '得点

    Dim IShipX As Integer       '船横軸（右方向に正、左方向に負）
    Dim IShipY As Integer       '船縦軸（下方向に正、上方向に負）
    Dim IsShipUp As Boolean     '上矢印が押されているか
    Dim IsShipDown As Boolean   '下矢印が押されているか
    Dim IsShipLeft As Boolean   '左矢印が押されているか
    Dim IsShipRight As Boolean  '右矢印が押されているか

    Dim IUfoX As Integer            '船横軸（右方向に正、左方向に負）
    ReadOnly IUfoY As Integer = 50  '船縦軸（固定）
    Dim IsUfoGoLeft As Boolean      'UFOを左に移動させる
    Dim IsUfoGoRight As Boolean     'UFOを右に移動させる

    Dim IMissileX As Integer    'ミサイル横軸
    Dim IMissileY As Integer    'ミサイル縦軸

    'タイマー処理
    Private Sub Timer20_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick

        '描画されているものを消して全面を白くする。
        mainGraphics.Clear(Color.White)

        'UFO制御
        ControlUfo()

        '船制御
        ControlShip()

        'ミサイル制御
        ControlMissile()

        '爆弾制御
        ControlBomb()

    End Sub

    'キーを押したときの判定
    Private Sub Form20_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown

        ' 入力内容
        Select Case e.KeyCode

            ' 「↑」キー
            Case Keys.Up
                IsShipUp = True

            ' 「↓」キー
            Case Keys.Down
                IsShipDown = True

            ' 「←」キー
            Case Keys.Left
                IsShipLeft = True

            ' 「→」キー
            Case Keys.Right
                IsShipRight = True
        End Select
    End Sub

    'キーを離したときの判定
    Private Sub Form20_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp

        ' 入力内容
        Select Case e.KeyCode

            ' 「↑」キー
            Case Keys.Up
                IsShipUp = False

            ' 「↓」キー
            Case Keys.Down
                IsShipDown = False

            ' 「←」キー
            Case Keys.Left
                IsShipLeft = False

            ' 「→」キー
            Case Keys.Right
                IsShipRight = False
        End Select
    End Sub

    'マウス左クリック判定
    Private Sub Form20_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseClick

        If e.Button = MouseButtons.Left Then

            'ミサイル画像ファイルを読み込む。
            MissileImage = Image.FromFile(_PATH + _MISSILE)

            '船の先端位置にミサイルを表示する。
            IMissileX = IShipX
            IMissileY = IShipY - MissileImage.Height
            mainGraphics.DrawImage(MissileImage, IMissileX, IMissileY)
        End If
    End Sub

    '船制御
    Private Sub ControlShip()

        '当たり判定
        If HitShipUfo() Then
            '減点する。
            IScore -= _SUBSTRACTION
        End If

        ' 上移動
        If IsShipUp Then
            If IShipY < ShipImage.Height Then
                IShipY = 0
            Else
                IShipY -= ShipImage.Height
            End If
        End If

        ' 下移動
        If IsShipDown AndAlso IShipY < ClientSize.Height - ShipImage.Height Then
            IShipY += ShipImage.Height
        End If

        ' 左移動
        If IsShipLeft AndAlso IShipX >= ShipImage.Width Then
            IShipX -= ShipImage.Width
        End If

        ' 右移動
        If IsShipRight AndAlso IShipX <= ClientSize.Width - ShipImage.Width - ShipImage.Width / 2 Then
            IShipX += ShipImage.Width
        End If

        ' 船を描画する。
        mainGraphics.DrawImage(ShipImage, IShipX, IShipY)
    End Sub

    'UFO制御
    Private Sub ControlUfo()

        '左移動
        If IsUfoGoLeft AndAlso IUfoX >= UfoImage.Width / 4 Then
            IUfoX -= UfoImage.Width / 4
        End If

        '右移動
        If IsUfoGoRight AndAlso IUfoX <= ClientSize.Width - UfoImage.Width Then
            IUfoX += UfoImage.Width / 4
        End If

        '左の限界に達したら右に折り返す。
        If IUfoX < UfoImage.Width / 4 Then
            IsUfoGoLeft = False
            IsUfoGoRight = True
        End If

        '右の限界に達したら左に折り返す。
        If IUfoX > ClientSize.Width - UfoImage.Width Then
            IsUfoGoLeft = True
            IsUfoGoRight = False
        End If

        'UFOを描画する。
        mainGraphics.DrawImage(UfoImage, IUfoX, IUfoY)
    End Sub

    'ミサイル制御
    Private Sub ControlMissile()

        ' ミサイルが存在しない場合、処理を行わない。
        If MissileImage Is Nothing Then
            Return
        End If

        '上移動
        IMissileY -= MissileImage.Height / 2

        'ミサイルを描画する。
        mainGraphics.DrawImage(MissileImage, IMissileX, IMissileY)
    End Sub


    '船UFO当たり判定
    Private Function HitShipUfo() As Boolean

        '戻り値の変数を生成する。
        Dim isHit = False

        If IsShipUp Then        '上に移動する場合
            If IUfoX <= IShipX + ShipImage.Width AndAlso IUfoX + UfoImage.Width >= IShipX AndAlso   'UFOの横幅に船の座標が含まれる
                IUfoY <= IShipY AndAlso IUfoY + UfoImage.Height >= IShipY - ShipImage.Height Then   'UFOの縦幅に船の座標が含まれる
                '戻り値にTrueを設定する。
                isHit = True
            End If

        ElseIf IsShipDown Then  '下に移動する場合
            If IUfoX <= IShipX + ShipImage.Width AndAlso IUfoX + UfoImage.Width >= IShipX AndAlso                           'UFOの横幅に船の座標が含まれる
                IUfoY <= IShipY + ShipImage.Height * 2 AndAlso IUfoY + UfoImage.Height >= IShipY + ShipImage.Height Then    'UFOの縦幅に船の座標が含まれる
                '戻り値にTrueを設定する。
                isHit = True
            End If

        ElseIf IsShipLeft Then  '左に移動する場合
            If IUfoX <= IShipX AndAlso IUfoX + UfoImage.Width >= IShipX - ShipImage.Width AndAlso   'UFOの横幅に船の座標が含まれる
                IUfoY <= IShipY + ShipImage.Height AndAlso IUfoY + UfoImage.Height >= IShipY Then   'UFOの縦幅に船の座標が含まれる
                '戻り値にTrueを設定する。
                isHit = True
            End If

        ElseIf IsShipRight Then '右に移動する場合
            If IUfoX <= IShipX + ShipImage.Width * 2 AndAlso IUfoX + UfoImage.Width >= IShipX + ShipImage.Width AndAlso     'UFOの横幅に船の座標が含まれる
                IUfoY <= IShipY + ShipImage.Height AndAlso IUfoY + UfoImage.Height >= IShipY Then                           'UFOの縦幅に船の座標が含まれる
                '戻り値にTrueを設定する。
                isHit = True
            End If

        Else                    'いずれにも移動しない場合
            If IUfoX <= IShipX + ShipImage.Width AndAlso IUfoX + UfoImage.Width >= IShipX AndAlso   'UFOの横幅に船の座標が含まれる
                IUfoY <= IShipY + ShipImage.Height AndAlso IUfoY + UfoImage.Height >= IShipY Then   'UFOの縦幅に船の座標が含まれる
                '戻り値にTrueを設定する。
                isHit = True
            End If
        End If

        '戻り値の変数を返却する。
        Return isHit
    End Function

    '初期表示処理
    Private Sub Form20_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '初回のみ描画用のGraphicsクラスのインスタンスを生成する。
        If mainGraphics Is Nothing Then
            Dim bmp As New Bitmap(ClientRectangle.Width, ClientRectangle.Height)
            BackgroundImage = bmp
            mainGraphics = Graphics.FromImage(bmp)
        End If

        '船とUFOの画像ファイルを読み込む。
        ShipImage = Image.FromFile(_PATH + _SHIP)
        UfoImage = Image.FromFile(_PATH + _UFO)

        '船の初期表示座標を定める。
        IShipX = ClientSize.Width / 2 - ShipImage.Width / 2     '中央
        IShipY = ClientSize.Height - ShipImage.Height           '底辺

        'スコア表示
        PrintScore()
    End Sub

    'スコア表示
    Private Sub PrintScore()
        'ゼロ埋めして表示する。
        Label1.Text = _SCORE + IScore.ToString(_ZERO_PADDING)
    End Sub

    '爆弾制御
    Private Sub ControlBomb()

        'ミサイルがUFOに当たった場合
        If MissileImage IsNot Nothing AndAlso                                                           'ミサイルオブジェクトが存在する
            IUfoX <= IMissileX + MissileImage.Width AndAlso IUfoX + UfoImage.Width >= IMissileX AndAlso 'UFOの横幅にミサイルの座標が含まれる
            IUfoY <= IMissileY + MissileImage.Height AndAlso IUfoY + UfoImage.Height >= IMissileY Then  'UFOの縦幅にミサイルの座標が含まれる

            '爆弾画像ファイルを読込む。
            BombImage = Image.FromFile(_PATH + _BOMB)
            mainGraphics.DrawImage(BombImage, IMissileX, IMissileY)

            '加点する。
            IScore += _ADDITION

            'ミサイルと爆弾のオブジェクトを削除する。
            MissileImage = Nothing
            BombImage = Nothing
        End If

        'スコア表示
        PrintScore()
    End Sub
End Class
